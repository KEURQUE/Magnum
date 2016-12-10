using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Magnum.Controls.Breadcrumb
{
  public class PathExistsValidationRule : ValidationRule
  {
    #region Constructor

    public PathExistsValidationRule(Magnum.Controls.Breadcrumb.SuggestBox.IHierarchyHelper hierarchyHelper, object root)
    {
      _hierarchyHelper = hierarchyHelper;
      _root = root;
    }

    public PathExistsValidationRule()
    {

    }

    #endregion

    #region Methods

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      try
      {
        if (!(value is string))
          return new ValidationResult(false, "Invalid Path");

        if (_hierarchyHelper.GetItem(_root, (string)value) == null)
          return new ValidationResult(false, "Path Not Found");
      }
      catch (Exception)
      {
        return new ValidationResult(false, "Invalid Path");
      }
      return new ValidationResult(true, null);
    }

    #endregion

    #region Data

    Magnum.Controls.Breadcrumb.SuggestBox.IHierarchyHelper _hierarchyHelper;
    object _root;

    #endregion

    #region Public Properties

    #endregion
  }

    /// <summary>
    /// Uses ISuggestSource and HierarchyHelper to suggest automatically.
    /// </summary>
    public class SuggestBox : SuggestBoxBase
    {
        #region Constructor

        public SuggestBox()
        {
        }

        #endregion

        #region Methods

      

        #region Utils - Update Bindings

       
        protected override void updateSource()
        {
            var txtBindingExpr = this.GetBindingExpression(TextBox.TextProperty);
            if (txtBindingExpr == null)
                return;

            bool valid = true;
            if (HierarchyHelper != null)
                valid = HierarchyHelper.GetItem(RootItem, Text) != null;

            if (valid)
            {
                if (txtBindingExpr != null)
                    txtBindingExpr.UpdateSource();
                RaiseEvent(new RoutedEventArgs(ValueChangedEvent));                
            }
            else Validation.MarkInvalid(txtBindingExpr,                
                new ValidationError(new PathExistsValidationRule(), txtBindingExpr, 
                    "Path not exists.", null));
        }

        #endregion

   

        #region OnEventHandler

   
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            var suggestSources = SuggestSources;
            var hierarchyHelper = HierarchyHelper;
            string text = Text;
            object data = RootItem;
            IsHintVisible = String.IsNullOrEmpty(text);

            if (IsEnabled && suggestSources != null)
                Task.Run(async () =>
                    {
                        var tasks = from s in suggestSources select
                                     s.SuggestAsync(data, text, hierarchyHelper);
                        await Task.WhenAll(tasks);
                        return tasks.SelectMany(tsk => tsk.Result).Distinct().ToList();
                    }).ContinueWith(
                    (pTask) =>
                    {
                        if (!pTask.IsFaulted)
                            this.SetValue(SuggestionsProperty, pTask.Result);
                    }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        #endregion

        #region Data

        #endregion

        #region Public Properties


        #region HierarchyHelper, SuggestSource


        public interface IHierarchyHelper
        {
          /// <summary>
          /// Used to generate ItemsSource for BreadcrumbCore.
          /// </summary>
          /// <param name="item"></param>
          /// <returns></returns>
          IEnumerable<object> GetHierarchy(object item, bool includeCurrent);

          /// <summary>
          /// Generate Path from an item;
          /// </summary>
          /// <param name="item"></param>
          /// <returns></returns>
          string GetPath(object item);

          /// <summary>
          /// Get Item from path.
          /// </summary>
          /// <param name="rootItem">RootItem or ItemSource which can be used to lookup from.</param>
          /// <param name="path"></param>
          /// <returns></returns>
          object GetItem(object rootItem, string path);

          IEnumerable List(object item);

          string ExtractPath(string pathName);

          string ExtractName(string pathName);

          char Separator { get; }
          StringComparison StringComparisonOption { get; }

          string ParentPath { get; }
          string ValuePath { get; }
          string SubentriesPath { get; }
        }


        public interface ISuggestSource
        {
          Task<IList<object>> SuggestAsync(object data, string input, IHierarchyHelper helper);
        }


        /// <summary>
        /// Use Path to query for hierarchy of ViewModels.
        /// </summary>
        public class PathHierarchyHelper : IHierarchyHelper
        {
          #region Constructor

          public PathHierarchyHelper(string parentPath, string valuePath, string subEntriesPath)
          {
            ParentPath = parentPath;
            ValuePath = valuePath;
            SubentriesPath = subEntriesPath;
            Separator = '\\';
            StringComparisonOption = StringComparison.CurrentCultureIgnoreCase;
          }

          #endregion

          #region Methods

          #region Utils Func - extractPath/Name
          public virtual string ExtractPath(string pathName)
          {
            if (String.IsNullOrEmpty(pathName))
              return "";
            if (pathName.IndexOf(Separator) == -1)
              return "";
            else return pathName.Substring(0, pathName.LastIndexOf(Separator));
          }

          public virtual string ExtractName(string pathName)
          {
            if (String.IsNullOrEmpty(pathName))
              return "";
            if (pathName.IndexOf(Separator) == -1)
              return pathName;
            else return pathName.Substring(pathName.LastIndexOf(Separator) + 1);
          }
          #endregion

          #region Overridable to improve speed.

          protected virtual object getParent(object item)
          {
            return PropertyPathHelper.GetValueFromPropertyInfo(item, ParentPath);
          }

          protected virtual string getValuePath(object item)
          {
            return PropertyPathHelper.GetValueFromPropertyInfo(item, ValuePath) as string;
          }

          protected virtual IEnumerable getSubEntries(object item)
          {
            return PropertyPathHelper.GetValueFromPropertyInfo(item, SubentriesPath) as IEnumerable;
          }

          #endregion

          #region Implements

          public IEnumerable<object> GetHierarchy(object item, bool includeCurrent)
          {
            if (includeCurrent)
              yield return item;

            var current = getParent(item);
            while (current != null)
            {
              yield return current;
              current = getParent(current);
            }
          }

          public string GetPath(object item)
          {
            return item == null ? "" : getValuePath(item);
          }

          public IEnumerable List(object item)
          {
            return item is IEnumerable ? item as IEnumerable : getSubEntries(item);
          }

          public object GetItem(object rootItem, string path)
          {
            var queue = new Queue<string>(path.Split(new char[] { Separator }, StringSplitOptions.RemoveEmptyEntries));
            object current = rootItem;
            while (current != null && queue.Any())
            {
              var nextSegment = queue.Dequeue();
              object found = null;
              foreach (var item in List(current))
              {
                string valuePathName = getValuePath(item);
                string value = ExtractName(valuePathName); //Value may be full path, or just current value.
                if (value.Equals(nextSegment, StringComparisonOption))
                {
                  found = item;
                  break;
                }
              }
              current = found;
            }
            return current;
          }

          #endregion

          #endregion

          #region Data

          #endregion

          #region Public Properties

          public char Separator { get; set; }
          public StringComparison StringComparisonOption { get; set; }
          public string ParentPath { get; set; }
          public string ValuePath { get; set; }
          public string SubentriesPath { get; set; }

          #endregion
        }


        //Thomas Levesque - http://stackoverflow.com/questions/3577802/wpf-getting-a-property-value-from-a-binding-path
        public static class PropertyPathHelper
        {
          internal static Dictionary<Tuple<Type, string>, PropertyInfo> _cacheDic
              = new Dictionary<Tuple<Type, string>, PropertyInfo>();
          public static object GetValueFromPropertyInfo(object obj, string[] propPaths)
          {
            var current = obj;
            foreach (var ppath in propPaths)
            {
              if (current == null)
                return null;

              Type type = current.GetType();
              var key = new Tuple<Type, string>(type, ppath);

              PropertyInfo pInfo = null;
              lock (_cacheDic)
              {
                if (!(_cacheDic.ContainsKey(key)))
                {
                  pInfo = type.GetProperty(ppath);
                  _cacheDic.Add(key, pInfo);
                }
                pInfo = _cacheDic[key];
              }

              if (pInfo == null)
                return null;
              current = pInfo.GetValue(current);
            }
            return current;
          }

          public static object GetValueFromPropertyInfo(object obj, string propertyPath)
          {
            return GetValueFromPropertyInfo(obj, propertyPath.Split('.'));
          }

          public static object GetValue(object obj, string propertyPath)
          {
            Dispatcher dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
            if (dispatcher == null)
              return GetValueFromPropertyInfo(obj, propertyPath);

            Binding binding = new Binding(propertyPath);
            binding.Mode = BindingMode.OneTime;
            binding.Source = obj;
            BindingOperations.SetBinding(_dummy, Dummy.ValueProperty, binding);
            return _dummy.GetValue(Dummy.ValueProperty);


          }

          public static object GetValue(object obj, BindingBase binding)
          {
            return GetValue(obj, (binding as Binding).Path.Path);
          }



          private static readonly Dummy _dummy = new Dummy();

          private class Dummy : DependencyObject
          {
            public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof(object), typeof(Dummy), new UIPropertyMetadata(null));
          }
        }


        /// <summary>
        /// Suggest based on sub entries of specified data.
        /// </summary>
        public class AutoSuggestSource : ISuggestSource
        {
          #region Constructor

          public AutoSuggestSource()
          {
          }


          #endregion

          #region Methods


          public Task<IList<object>> SuggestAsync(object data, string input, IHierarchyHelper helper)
          {
            if (helper == null)
              return Task.FromResult<IList<object>>(new List<Object>());

            string valuePath = helper.ExtractPath(input);
            string valueName = helper.ExtractName(input);
            if (String.IsNullOrEmpty(valueName) && input.EndsWith(helper.Separator + ""))
              valueName += helper.Separator;

            if (valuePath == "" && input.EndsWith("" + helper.Separator))
              valuePath = valueName;
            var found = helper.GetItem(data, valuePath);
            List<object> retVal = new List<object>();

            if (found != null)
            {
              foreach (var item in helper.List(found))
              {
                string valuePathName = helper.GetPath(item) as string;
                if (valuePathName.StartsWith(input, helper.StringComparisonOption) &&
                    !valuePathName.Equals(input, helper.StringComparisonOption))
                  retVal.Add(item);
              }
            }



            return Task.FromResult<IList<object>>(retVal);

          }

          #endregion

          #region Data

          #endregion

          #region Public Properties

          #endregion
        }

        public IHierarchyHelper HierarchyHelper
        {
            get { return (IHierarchyHelper)GetValue(HierarchyHelperProperty); }
            set { SetValue(HierarchyHelperProperty, value); }
        }

        public static readonly DependencyProperty HierarchyHelperProperty =
            DependencyProperty.Register("HierarchyHelper", typeof(IHierarchyHelper),
            typeof(SuggestBox), new UIPropertyMetadata(new PathHierarchyHelper("Parent", "Value", "SubEntries")));

        public static readonly DependencyProperty SuggestSourcesProperty = DependencyProperty.Register(
            "SuggestSources", typeof(IEnumerable<ISuggestSource>), typeof(SuggestBox), new PropertyMetadata(
                new List<ISuggestSource>(new [] { new AutoSuggestSource() })));

        public IEnumerable<ISuggestSource> SuggestSources
        {
            get { return (IEnumerable<ISuggestSource>)GetValue(SuggestSourcesProperty); }
            set { SetValue(SuggestSourcesProperty, value); }
        }
        #endregion


        public static readonly DependencyProperty RootItemProperty = DependencyProperty.Register("RootItem",
            typeof(object), typeof(SuggestBox), new PropertyMetadata(null));

        /// <summary>
        /// Assigned by Breadcrumb
        /// </summary>
        public object RootItem
        {
            get { return GetValue(RootItemProperty); }
            set { SetValue(RootItemProperty, value); }
        }


        #endregion
    }
}
