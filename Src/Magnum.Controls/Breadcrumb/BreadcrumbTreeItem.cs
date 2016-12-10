using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Magnum.Controls.Breadcrumb
{
  public class OverflowableStackPanel : StackPanel
  {
    #region Constructor

    #endregion

    #region Methods

    private double getWH(Size size, Orientation orientation)
    {
      return orientation == Orientation.Horizontal ? size.Width : size.Height;
    }

    private double getHW(Size size, Orientation orientation)
    {
      return orientation == Orientation.Vertical ? size.Width : size.Height;
    }


    protected override Size MeasureOverride(Size constraint)
    {
      //if (double.IsPositiveInfinity(constraint.Width) || double.IsPositiveInfinity(constraint.Height))
      //    return base.MeasureOverride(constraint);

      var items = InternalChildren.Cast<UIElement>();

      overflowableWH = 0;
      nonoverflowableWH = 0;
      int overflowCount = 0;
      double maxHW = 0;

      foreach (var item in items)
      {
        item.Measure(constraint);
        maxHW = Math.Max(getHW(item.DesiredSize, Orientation), maxHW);
        if (GetCanOverflow(item))
          overflowableWH += getWH(item.DesiredSize, Orientation);
        else nonoverflowableWH += getWH(item.DesiredSize, Orientation);
      }

      foreach (var ele in items.Reverse())
      {
        if (GetCanOverflow(ele))
          if (overflowableWH + nonoverflowableWH > getWH(constraint, Orientation))
          {
            overflowCount += 1;
            SetIsOverflow(ele, true);
            overflowableWH -= getWH(ele.DesiredSize, Orientation);
          }
          else SetIsOverflow(ele, false);
      }

      SetValue(OverflowItemCountProperty, overflowCount);

      return Orientation == Orientation.Horizontal ?
              new Size(overflowableWH + nonoverflowableWH, maxHW) :
              new Size(maxHW, overflowableWH + nonoverflowableWH);

    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
      var items = InternalChildren.Cast<UIElement>();
      if (Orientation == Orientation.Horizontal)
      {
        double curX = 0;
        foreach (var item in items)
        {
          if (!GetCanOverflow(item) || !GetIsOverflow(item)) //Not overflowable or not set overflow
          {
            item.Arrange(new Rect(curX, 0, item.DesiredSize.Width, arrangeSize.Height));
            curX += item.DesiredSize.Width;
          }
          else item.Arrange(new Rect(0, 0, 0, 0));

        }
        return arrangeSize;
      }
      else
        return base.ArrangeOverride(arrangeSize);


    }

    #endregion

    #region Data

    double overflowableWH = 0;
    double nonoverflowableWH = 0;

    #endregion

    #region Public Properties

    public static DependencyProperty OverflowItemCountProperty = DependencyProperty.Register("OverflowItemCount", typeof(int),
        typeof(OverflowableStackPanel), new PropertyMetadata(0));

    public int OverflowItemCount
    {
      get { return (int)GetValue(OverflowItemCountProperty); }
      set { SetValue(OverflowItemCountProperty, value); }
    }

    public static DependencyProperty CanOverflowProperty = DependencyProperty.RegisterAttached("CanOverflow", typeof(bool),
        typeof(OverflowableStackPanel), new UIPropertyMetadata(false));

    public static bool GetCanOverflow(DependencyObject obj)
    {
      return (bool)obj.GetValue(CanOverflowProperty);
    }

    public static void SetCanOverflow(DependencyObject obj, bool value)
    {
      obj.SetValue(CanOverflowProperty, value);
    }

    public static DependencyProperty IsOverflowProperty = DependencyProperty.RegisterAttached("IsOverflow", typeof(bool),
       typeof(OverflowableStackPanel), new UIPropertyMetadata(false));

    public static bool GetIsOverflow(DependencyObject obj)
    {
      return (bool)obj.GetValue(IsOverflowProperty);
    }

    public static void SetIsOverflow(DependencyObject obj, bool value)
    {
      obj.SetValue(IsOverflowProperty, value);
    }

    #endregion
  }

    [TemplateVisualState(Name = "ShowCaption", GroupName = "CaptionStates")]
    [TemplateVisualState(Name = "HideCaption", GroupName = "CaptionStates")]
    public class BreadcrumbTreeItem : TreeViewItem
    {
        #region Constructor

        static BreadcrumbTreeItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BreadcrumbTreeItem),
                new FrameworkPropertyMetadata(typeof(BreadcrumbTreeItem)));
        }

        public BreadcrumbTreeItem()
        {
     
        }

        #endregion

        #region Methods

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new BreadcrumbTreeItem();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.AddHandler(Button.ClickEvent, (RoutedEventHandler)((o, e) =>
                {
                    if (e.Source is Button)
                    {
                        this.SetValue(IsCurrentSelectedProperty, true);
                        e.Handled = true;
                    }
                }));

            //this.AddHandler(OverflowItem.SelectedEvent, (RoutedEventHandler)((o, e) =>
            //    {
            //        if (e.Source is OverflowItem)
            //        {
            //            IsExpanded = false;
            //        }
            //    }));
        }

        public static void OnIsCaptionVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as BreadcrumbTreeItem).UpdateStates(true);
        }

        public static void OnOverflowItemCountChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as BreadcrumbTreeItem).SetValue(IsOverflowedProperty, ((int)args.NewValue) > 0);
        }

        private void UpdateStates(bool useTransition)
        {
            if (IsCaptionVisible)
                VisualStateManager.GoToState(this, "ShowCaption", useTransition);
            else VisualStateManager.GoToState(this, "HideCaption", useTransition);
        }

        #endregion

        #region Data
        
        #endregion

        #region Public Properties

        public static DependencyProperty OverflowItemCountProperty = OverflowableStackPanel.OverflowItemCountProperty
            .AddOwner(typeof(BreadcrumbTreeItem), new PropertyMetadata(OnOverflowItemCountChanged));

        public int OverflowItemCount
        {
            get { return (int)GetValue(OverflowItemCountProperty); }
            set { SetValue(OverflowItemCountProperty, value); }
        }

        public static DependencyProperty IsOverflowedProperty = DependencyProperty.Register("IsOverflowed", typeof(bool),
         typeof(BreadcrumbTreeItem), new PropertyMetadata(false));

        public bool IsOverflowed
        {
            get { return (bool)GetValue(IsOverflowedProperty); }
            set { SetValue(IsOverflowedProperty, value); }
        }

        public static readonly DependencyProperty OverflowedItemContainerStyleProperty =
                BreadcrumbTree.OverflowedItemContainerStyleProperty.AddOwner(typeof(BreadcrumbTreeItem));

        public Style OverflowedItemContainerStyle
        {
            get { return (Style)GetValue(OverflowedItemContainerStyleProperty); }
            set { SetValue(OverflowedItemContainerStyleProperty, value); }
        }

        public static readonly DependencyProperty SelectedChildProperty =
          DependencyProperty.Register("SelectedChild", typeof(object), typeof(BreadcrumbTreeItem),
              new UIPropertyMetadata(null));

        public object SelectedChild
        {
            get { return (object)GetValue(SelectedChildProperty); }
            set { SetValue(SelectedChildProperty, value); }
        }

        public static readonly DependencyProperty ValuePathProperty =
         DependencyProperty.Register("ValuePath", typeof(string), typeof(BreadcrumbTreeItem),
             new UIPropertyMetadata(""));

        public string ValuePath
        {
            get { return (string)GetValue(ValuePathProperty); }
            set { SetValue(ValuePathProperty, value); }
        }
        

        public static readonly DependencyProperty IsChildSelectedProperty =
            DependencyProperty.Register("IsChildSelected", typeof(bool), typeof(BreadcrumbTreeItem), 
                new UIPropertyMetadata());

        public bool IsChildSelected
        {
            get { return (bool)GetValue(IsChildSelectedProperty); }
            set { SetValue(IsChildSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsCurrentSelectedProperty =
           DependencyProperty.Register("IsCurrentSelected", typeof(bool), typeof(BreadcrumbTreeItem),
               new UIPropertyMetadata(false));

        public bool IsCurrentSelected
        {
            get { return (bool)GetValue(IsCurrentSelectedProperty); }
            set { SetValue(IsCurrentSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsCaptionVisibleProperty =
                    DependencyProperty.Register("IsCaptionVisible", typeof(bool), typeof(BreadcrumbTreeItem),
                    new UIPropertyMetadata(true, OnIsCaptionVisibleChanged));

        /// <summary>
        /// Display Caption
        /// </summary>
        public bool IsCaptionVisible
        {
            get { return (bool)GetValue(IsCaptionVisibleProperty); }
            set { SetValue(IsCaptionVisibleProperty, value); }
        }

        public static readonly DependencyProperty MenuItemTemplateProperty =
                BreadcrumbTree.MenuItemTemplateProperty.AddOwner(typeof(BreadcrumbTreeItem));

        public DataTemplate MenuItemTemplate
        {
            get { return (DataTemplate)GetValue(MenuItemTemplateProperty); }
            set { SetValue(MenuItemTemplateProperty, value); }
        }

        
        #endregion
    }
}
