using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Media;

namespace Magnum.Controls.Breadcrumb
{

  public static partial class UIEventHubProperties
  {

    /// <summary>
    /// Skip lookup in UITools.FindAncestor, FindLogicalAncestor and FindVisualChild
    /// </summary>
    public static DependencyProperty SkipLookupProperty =
        DependencyProperty.RegisterAttached("SkipLookup", typeof(bool), typeof(UIEventHubProperties), new PropertyMetadata(false));

    public static bool GetSkipLookup(DependencyObject target)
    {
      return (bool)target.GetValue(SkipLookupProperty);
    }

    public static void SetSkipLookup(DependencyObject target, bool value)
    {
      target.SetValue(SkipLookupProperty, value);
    }




    /// <summary>
    /// Allow ControlUtils.GetScrollContentPresenter to cache the object to the control.
    /// </summary>
    public static DependencyProperty ScrollContentPresenterProperty =
        DependencyProperty.RegisterAttached("ScrollContentPresenter", typeof(ScrollContentPresenter),
        typeof(UIEventHubProperties), new PropertyMetadata(null));

    public static ScrollContentPresenter GetScrollContentPresenter(DependencyObject target)
    {
      return (ScrollContentPresenter)target.GetValue(ScrollContentPresenterProperty);
    }

    public static void SetScrollContentPresenter(DependencyObject target, ScrollContentPresenter value)
    {
      target.SetValue(ScrollContentPresenterProperty, value);
    }


    #region EnableDrag / Drop

    public static DependencyProperty EnableDragProperty =
       DependencyProperty.RegisterAttached("EnableDrag", typeof(bool), typeof(UIEventHubProperties));

    public static bool GetEnableDrag(DependencyObject target)
    {
      return (bool)target.GetValue(EnableDragProperty);
    }

    public static void SetEnableDrag(DependencyObject target, bool value)
    {
      target.SetValue(EnableDragProperty, value);
    }


    public static DependencyProperty EnableDropProperty =
        DependencyProperty.RegisterAttached("EnableDrop", typeof(bool), typeof(UIEventHubProperties));


    public static bool GetEnableDrop(DependencyObject target)
    {
      return (bool)target.GetValue(EnableDropProperty);
    }

    public static void SetEnableDrop(DependencyObject target, bool value)
    {
      target.SetValue(EnableDropProperty, value);
    }

    #endregion







    public static bool IsValidPosition(this Point point)
    {
      return point.X != double.NaN && point.Y != double.NaN;
    }

    public static bool HitDragThreshold(this Point position, Point startPosition)
    {
      return
          startPosition.IsValidPosition() &&
          Math.Abs(position.X - startPosition.X) > SystemParameters.MinimumHorizontalDragDistance ||
               Math.Abs(position.Y - startPosition.Y) > SystemParameters.MinimumVerticalDragDistance;
    }










  }

  public class BreadcrumbCorePanel : Panel
  {

    private Size lastfinalSize;


    protected override Size MeasureOverride(Size availableSize)
    {
      if (availableSize.Height == double.PositiveInfinity)
        availableSize = new Size(availableSize.Width, 30);
      Size resultSize = new Size(0, 0);

      if (Children.Count > 0)
      {
        //Measure as horizontal stack, right to left.
        double availableWidth = availableSize.Width;
        double maxHeight = 0;
        for (int i = Children.Count - 1; i >= 0; i--) //Allocate from last to first
        {
          var current = Children[i];
          current.Measure(new Size(availableWidth, availableSize.Height));
          availableWidth -= current.DesiredSize.Width;
          maxHeight = Math.Max(maxHeight, current.DesiredSize.Height);
        }
        if (availableWidth <= 0)
          return new Size(availableSize.Width, maxHeight);
        return new Size(availableSize.Width - availableWidth + 1, availableSize.Height);

      }
      return new Size(0, 0);

    }

    #region AttachedProperties

    public static T FindAncestor<T>(DependencyObject obj, Func<T, bool> filter = null) where T : DependencyObject
    {
      filter = filter ?? (depObj => true);
      while (obj != null && obj is Visual)
      {

        T o = obj as T;

        if (o != null && filter(o))
          return o;

        obj = VisualTreeHelper.GetParent(obj);

        if (obj != null && UIEventHubProperties.GetSkipLookup(obj))
          obj = null;
      }
      return default(T);

    }


    private BreadcrumbCore findBreadcrumbCore()
    {
      var bcore = FindAncestor<BreadcrumbCore>(this);
      return bcore;
    }

    private int GetLastNonVisibleIndex()
    {
      var bcore = findBreadcrumbCore();
      return bcore == null ? -1 : bcore.LastNonVisible;
    }


    public void SetLastNonVisibleIndex(int value)
    {
      var bcore = findBreadcrumbCore();
      if (bcore != null)
        bcore.SetValue(BreadcrumbCore.LastNonVisibleIndexProperty, value);
    }

    public void SetLastNonVisibleIndex()
    {
      var bcore = findBreadcrumbCore();
      if (bcore != null)
        bcore.SetValue(BreadcrumbCore.LastNonVisibleIndexProperty, bcore.DefaultLastNonVisibleIndex);
    }

    #endregion

    private Rect[] arrangeChildren(Size availableSize)
    {
      Rect[] retVal = new Rect[Children.Count];

      double curX = availableSize.Width;
      SetLastNonVisibleIndex();
      for (int i = Children.Count - 1; i >= 0; i--) //Allocate from last to first
      {
        var current = Children[i];
        if (curX > 0)
        {
          double startPos = curX - current.DesiredSize.Width;

          if (curX > current.DesiredSize.Width)
          {
            retVal[i] = new Rect(startPos,
               0,
                current.DesiredSize.Width, availableSize.Height);
            curX -= current.DesiredSize.Width;
          }
          else //Not enough space to allocate current, recalculate the retVal
          {
            retVal[i] = new Rect(0, 0, 0, 0);
            for (int j = i; j < Children.Count; j++)
              retVal[j].Offset(-curX, 0);
            curX = 0;
            SetLastNonVisibleIndex(i);
          }
        }
        else retVal[i] = new Rect(0, 0, 0, 0);
      }
      return retVal;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      lastfinalSize = finalSize;
      var rects = arrangeChildren(finalSize);
      Rect emptyRect = new Rect(0, 0, 0, 0);

      for (int i = 0; i < this.Children.Count; i++)
      {
        Children[i].Arrange(rects[i]);
        Children[i].SetValue(BreadcrumbItem.IsOverflowedProperty, rects[i] == emptyRect);
      }

      return finalSize;
    }
  }
}
