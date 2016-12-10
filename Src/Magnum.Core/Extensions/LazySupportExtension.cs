using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Magnum.Core.Extensions
{
  /// <summary>
  /// Adds <see cref="Lazy{T}"/> and <see cref="IEnumerable{T}"/> support for Unity.
  /// </summary>
  public class LazySupportExtension : UnityContainerExtension
  {
    protected override void Initialize()
    {
      // Enumerable strategy
      Context.Strategies.AddNew<EnumerableResolutionStrategy>(
          UnityBuildStage.TypeMapping);

      // Lazy policy
      Context.Policies.Set<IBuildPlanPolicy>(
          new LazyResolveBuildPlanPolicy(), typeof(Lazy<>));
    }
  }
}

/// <summary>
/// This strategy implements the logic that will return all instances
/// when a <see cref="IEnumerable{T}"/> parameter is detected.
/// </summary>
public class EnumerableResolutionStrategy : BuilderStrategy
{
  private delegate object Resolver(IBuilderContext context);

  private static readonly MethodInfo GenericResolveEnumerableMethod =
      typeof(EnumerableResolutionStrategy).GetMethod("ResolveEnumerable", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

  private static readonly MethodInfo GenericResolveLazyEnumerableMethod =
      typeof(EnumerableResolutionStrategy).GetMethod("ResolveLazyEnumerable", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

  /// <summary>
  /// Do the PreBuildUp stage of construction. This is where the actual work is performed.
  /// </summary>
  /// <param name="context">Current build context.</param>
  public override void PreBuildUp(IBuilderContext context)
  {
    Guard.ArgumentNotNull(context, "context");
    if (IsResolvingIEnumerable(context.BuildKey.Type))
    {
      MethodInfo resolverMethod;
      Type typeToBuild = GetTypeToBuild(context.BuildKey.Type);

      if (IsResolvingLazy(typeToBuild))
      {
        typeToBuild = GetTypeToBuild(typeToBuild);
        resolverMethod = GenericResolveLazyEnumerableMethod.MakeGenericMethod(typeToBuild);
      }
      else
      {
        resolverMethod = GenericResolveEnumerableMethod.MakeGenericMethod(typeToBuild);
      }

      var resolver = (Resolver)Delegate.CreateDelegate(typeof(Resolver), resolverMethod);
      context.Existing = resolver(context);
      context.BuildComplete = true;
    }
  }

  private static Type GetTypeToBuild(Type type)
  {
    return type.GetGenericArguments()[0];
  }

  private static bool IsResolvingIEnumerable(Type type)
  {
    return type.IsGenericType &&
           type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
  }

  private static bool IsResolvingLazy(Type type)
  {
    return type.IsGenericType &&
           type.GetGenericTypeDefinition() == typeof(Lazy<>);
  }

  private static object ResolveLazyEnumerable<T>(IBuilderContext context)
  {
    var container = context.NewBuildUp<IUnityContainer>();
    var typeToBuild = typeof(T);
    var typeWrapper = typeof(Lazy<T>);
    var results = ResolveAll(container, typeToBuild, typeWrapper).OfType<Lazy<T>>().ToList();

    return results;
  }

  private static object ResolveEnumerable<T>(IBuilderContext context)
  {
    var container = context.NewBuildUp<IUnityContainer>();
    var typeToBuild = typeof(T);
    var results = ResolveAll(container, typeToBuild, typeToBuild).OfType<T>().ToList();

    return results;
  }

  private static IEnumerable<object> ResolveAll(IUnityContainer container, Type type, Type typeWrapper)
  {
    var names = GetRegisteredNames(container, type);
    if (type.IsGenericType)
    {
      names = names.Concat(GetRegisteredNames(container, type.GetGenericTypeDefinition()));
    }

    return names.Distinct()
                .Select(t => t.Name)
                .Select(name => container.Resolve(typeWrapper, name));
  }

  private static IEnumerable<ContainerRegistration> GetRegisteredNames(IUnityContainer container, Type type)
  {
    return container.Registrations.Where(t => t.RegisteredType == type);
  }
}

// <summary>
    /// Build plan which enables true support for <see cref="Lazy{T}"/>.
    /// </summary>
public class LazyResolveBuildPlanPolicy : IBuildPlanPolicy
{
  public void BuildUp(IBuilderContext context)
  {
    if (context.Existing == null)
    {
      var currentContainer = context.NewBuildUp<IUnityContainer>();
      var typeToBuild = GetTypeToBuild(context.BuildKey.Type);
      var nameToBuild = context.BuildKey.Name;

      context.Existing = IsResolvingIEnumerable(typeToBuild) ?
          CreateResolveAllResolver(currentContainer, typeToBuild) :
          CreateResolver(currentContainer, typeToBuild, nameToBuild);

      DynamicMethodConstructorStrategy.SetPerBuildSingleton(context);
    }
  }

  private static Type GetTypeToBuild(Type type)
  {
    return type.GetGenericArguments()[0];
  }

  private static bool IsResolvingIEnumerable(Type type)
  {
    return type.IsGenericType &&
           type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
  }

  private static object CreateResolver(IUnityContainer currentContainer, Type typeToBuild,
      string nameToBuild)
  {
    Type lazyType = typeof(Lazy<>).MakeGenericType(typeToBuild);
    Type trampolineType = typeof(ResolveTrampoline<>).MakeGenericType(typeToBuild);
    Type delegateType = typeof(Func<>).MakeGenericType(typeToBuild);
    MethodInfo resolveMethod = trampolineType.GetMethod("Resolve");

    object trampoline = Activator.CreateInstance(trampolineType, currentContainer, nameToBuild);
    object trampolineDelegate = Delegate.CreateDelegate(delegateType, trampoline, resolveMethod);

    return Activator.CreateInstance(lazyType, trampolineDelegate);
  }

  private static object CreateResolveAllResolver(IUnityContainer currentContainer, Type enumerableType)
  {
    Type typeToBuild = GetTypeToBuild(enumerableType);
    Type lazyType = typeof(Lazy<>).MakeGenericType(enumerableType);
    Type trampolineType = typeof(ResolveAllTrampoline<>).MakeGenericType(typeToBuild);
    Type delegateType = typeof(Func<>).MakeGenericType(enumerableType);
    MethodInfo resolveAllMethod = trampolineType.GetMethod("ResolveAll");

    object trampoline = Activator.CreateInstance(trampolineType, currentContainer);
    object trampolineDelegate = Delegate.CreateDelegate(delegateType, trampoline, resolveAllMethod);

    return Activator.CreateInstance(lazyType, trampolineDelegate);
  }

  private class ResolveTrampoline<T>
  {
    private readonly IUnityContainer m_Container;
    private readonly string m_Name;

    public ResolveTrampoline(IUnityContainer container, string name)
    {
      m_Container = container;
      m_Name = name;
    }

    public T Resolve()
    {
      return m_Container.Resolve<T>(m_Name);
    }
  }

  private class ResolveAllTrampoline<T>
  {
    private readonly IUnityContainer m_Container;

    public ResolveAllTrampoline(IUnityContainer container)
    {
      m_Container = container;
    }

    public IEnumerable<T> ResolveAll()
    {
      return m_Container.ResolveAll<T>();
    }
  }
}
