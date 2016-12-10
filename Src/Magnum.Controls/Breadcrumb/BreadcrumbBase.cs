using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Magnum.Controls.Breadcrumb
{
  public class OneItemPanel : Panel
  {

    protected override Size MeasureOverride(Size availableSize)
    {
      for (int i = 0; i < InternalChildren.Count; i++)
      {
        UIElement element = InternalChildren[i];
        if (element.Visibility == System.Windows.Visibility.Visible)
        {
          element.Measure(availableSize);
          return element.DesiredSize;
        }
      }
      return new Size(0, 0);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      UIElement visibleElement = null;
      for (int i = 0; i < InternalChildren.Count; i++)
      {
        UIElement element = InternalChildren[i];
        if (visibleElement == null && element.Visibility == System.Windows.Visibility.Visible)
        {
          visibleElement = element;
          visibleElement.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
          break;
        }
        else
        {
          element.Arrange(new Rect(0, 0, 0, 0));
        }
      }


      return finalSize;
    }
  }

  [TemplatePart(Name = "hotTrackGrid", Type = typeof(Grid))]
  [TemplatePart(Name = "selected", Type = typeof(Rectangle))]
  [TemplatePart(Name = "background", Type = typeof(Rectangle))]
  [TemplatePart(Name = "highlight", Type = typeof(Rectangle))]
  [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Dragging", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Selected", GroupName = "CommonStates")]
  public class HotTrack : ContentControl
  {
    #region Constructor
    static HotTrack()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(HotTrack),
          new FrameworkPropertyMetadata(typeof(HotTrack)));
    }
    #endregion

    #region Methods

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      //UITools.AddValueChanged<HotTrack>(this, IsFocused, 
      UpdateStates(false);
    }

    //protected override void OnGotFocus(RoutedEventArgs e)
    //{
    //    base.OnGotFocus(e);
    //    UpdateStates(true);
    //}

    //protected override void OnLostFocus(RoutedEventArgs e)
    //{
    //    base.OnLostFocus(e);
    //    UpdateStates(true);
    //}

    protected override void OnMouseEnter(MouseEventArgs e)
    {
      base.OnMouseEnter(e);

      UpdateStates(true);
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
      base.OnMouseEnter(e);

      UpdateStates(true);
    }

    private void UpdateStates(bool useTransition)
    {

      if (IsSelected)
        VisualStateManager.GoToState(this, "Selected", useTransition);
      else if (IsDragging)
        VisualStateManager.GoToState(this, "Dragging", useTransition);
      else if (IsDraggingOver)
        VisualStateManager.GoToState(this, "DraggingOver", useTransition);
      else if (IsMouseOver)
        if (IsEnabled)
          VisualStateManager.GoToState(this, "MouseOver", useTransition);
        else VisualStateManager.GoToState(this, "MouseOverGrayed", useTransition);
      else VisualStateManager.GoToState(this, "Normal", useTransition);

      //if (IsFocused)
      //    VisualStateManager.GoToState(this, "Focused", useTransition);
      //else VisualStateManager.GoToState(this, "Unfocused", useTransition);
    }

    private static void OnIsSelectedChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
      HotTrack ht = (HotTrack)sender;
      if (!(args.NewValue.Equals(args.OldValue)))
        ht.UpdateStates(true);
    }

    #endregion

    #region Data

    #endregion

    #region Public Properties

    public bool IsSelected
    {
      get { return (bool)GetValue(IsSelectedProperty); }
      set { SetValue(IsSelectedProperty, value); }
    }

    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.Register("IsSelected", typeof(bool),
        typeof(HotTrack), new UIPropertyMetadata(false,
            new PropertyChangedCallback(OnIsSelectedChanged)));

    public Brush SelectedBorderBrush
    {
      get { return (Brush)GetValue(SelectedBorderBrushProperty); }
      set { SetValue(SelectedBorderBrushProperty, value); }
    }

    public static readonly DependencyProperty SelectedBorderBrushProperty =
        DependencyProperty.Register("SelectedBorderBrush", typeof(Brush),
        typeof(HotTrack), new UIPropertyMetadata(Brushes.Transparent));

    public Brush BackgroundBrush
    {
      get { return (Brush)GetValue(BackgroundBrushProperty); }
      set { SetValue(BackgroundBrushProperty, value); }
    }

    public static readonly DependencyProperty BackgroundBrushProperty =
        DependencyProperty.Register("BackgroundBrush", typeof(Brush),
        typeof(HotTrack), new UIPropertyMetadata(SystemColors.HotTrackBrush));


    public Brush SelectedBrush
    {
      get { return (Brush)GetValue(SelectedBrushProperty); }
      set { SetValue(SelectedBrushProperty, value); }
    }

    public static readonly DependencyProperty SelectedBrushProperty =
        DependencyProperty.Register("SelectedBrush", typeof(Brush),
        typeof(HotTrack), new UIPropertyMetadata(SystemColors.ActiveCaptionBrush));

    public Brush HighlightBrush
    {
      get { return (Brush)GetValue(HighlightBrushProperty); }
      set { SetValue(HighlightBrushProperty, value); }
    }

    public static readonly DependencyProperty HighlightBrushProperty =
        DependencyProperty.Register("HighlightBrush", typeof(Brush),
        typeof(HotTrack), new UIPropertyMetadata(new SolidColorBrush(Color.FromArgb(117, 255, 255, 255))));


    public CornerRadius CornerRadius
    {
      get { return (CornerRadius)GetValue(CornerRadiusProperty); }
      set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius),
        typeof(HotTrack), new UIPropertyMetadata(new CornerRadius(0)));


    public bool IsDragging
    {
      get { return (bool)GetValue(IsDraggingProperty); }
      set { SetValue(IsDraggingProperty, value); }
    }

    public static readonly DependencyProperty IsDraggingProperty =
        DependencyProperty.Register("IsDragging", typeof(bool),
        typeof(HotTrack), new UIPropertyMetadata(false, OnIsSelectedChanged));

    public bool IsDraggingOver
    {
      get { return (bool)GetValue(IsDraggingOverProperty); }
      set { SetValue(IsDraggingOverProperty, value); }
    }

    public static readonly DependencyProperty IsDraggingOverProperty =
        DependencyProperty.Register("IsDraggingOver", typeof(bool),
        typeof(HotTrack), new UIPropertyMetadata(false, OnIsSelectedChanged));

    /// <summary>
    /// For TreeView, create a mirror to completely highlight the whole row.
    /// </summary>
    public bool FillFullRow
    {
      get { return (bool)GetValue(FillFullRowProperty); }
      set { SetValue(FillFullRowProperty, value); }
    }

    public static readonly DependencyProperty FillFullRowProperty =
        DependencyProperty.Register("FillFullRow", typeof(bool),
        typeof(HotTrack), new UIPropertyMetadata(false));

    #endregion
  }

  /// <summary>
  /// Take multiple ContentTemplates (ContentTemplate, ContentTemplate2) and render it using the same Content.
  /// </summary>
  public class MultiContentControl : ContentControl
  {
    #region Constructor

    static MultiContentControl()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiContentControl),
          new FrameworkPropertyMetadata(typeof(MultiContentControl)));
    }

    #endregion

    #region Methods

    #endregion

    #region Data

    #endregion

    #region Public Properties

    public static readonly DependencyProperty ContentVisible1Property =
       DependencyProperty.Register("ContentVisible1", typeof(bool), typeof(MultiContentControl),
       new PropertyMetadata(true));

    public bool ContentVisible1
    {
      get { return (bool)GetValue(ContentVisible1Property); }
      set { SetValue(ContentVisible1Property, value); }
    }

    public static readonly DependencyProperty ContentTemplate2Property =
        DependencyProperty.Register("ContentTemplate2", typeof(DataTemplate), typeof(MultiContentControl),
        new PropertyMetadata(null));

    public DataTemplate ContentTemplate2
    {
      get { return (DataTemplate)GetValue(ContentTemplate2Property); }
      set { SetValue(ContentTemplate2Property, value); }
    }

    public static readonly DependencyProperty ContentVisible2Property =
        DependencyProperty.Register("ContentVisible2", typeof(bool), typeof(MultiContentControl),
        new PropertyMetadata(false));

    public bool ContentVisible2
    {
      get { return (bool)GetValue(ContentVisible2Property); }
      set { SetValue(ContentVisible2Property, value); }
    }


    public static readonly DependencyProperty ContentTemplate3Property =
        DependencyProperty.Register("ContentTemplate3", typeof(DataTemplate), typeof(MultiContentControl), new PropertyMetadata(null));

    public DataTemplate ContentTemplate3
    {
      get { return (DataTemplate)GetValue(ContentTemplate3Property); }
      set { SetValue(ContentTemplate3Property, value); }
    }

    public static readonly DependencyProperty ContentVisible3Property =
        DependencyProperty.Register("ContentVisible3", typeof(bool), typeof(MultiContentControl),
        new PropertyMetadata(true));

    public bool ContentVisible3
    {
      get { return (bool)GetValue(ContentVisible3Property); }
      set { SetValue(ContentVisible3Property, value); }
    }

    #endregion
  }

  /// <summary>
  /// Display ContentOn or ContentOff depends on whether IsSwitchOn is true.
  /// </summary>
  public class Switch : HeaderedContentControl
  {
    #region Constructor

    static Switch()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(Switch),
          new FrameworkPropertyMetadata(typeof(Switch)));
    }

    #endregion

    #region Methods

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      //this.AddHandler(HeaderedContentControl.MouseDownEvent, (RoutedEventHandler)((o, e) =>
      //    {
      //        this.SetValue(IsSwitchOnProperty, !IsSwitchOn);
      //    }));
    }

    public static void OnIsSwitchOnChanged(object sender, DependencyPropertyChangedEventArgs args)
    {

    }

    #endregion

    #region Data

    #endregion

    #region Public Properties

    public bool IsSwitchOn
    {
      get { return (bool)GetValue(IsSwitchOnProperty); }
      set { SetValue(IsSwitchOnProperty, value); }
    }

    public static readonly DependencyProperty IsSwitchOnProperty =
        DependencyProperty.Register("IsSwitchOn", typeof(bool),
        typeof(Switch), new UIPropertyMetadata(true, new PropertyChangedCallback(OnIsSwitchOnChanged)));

    public object ContentOn
    {
      get { return (object)GetValue(ContentOnProperty); }
      set { SetValue(ContentOnProperty, value); }
    }

    public static readonly DependencyProperty ContentOnProperty =
        DependencyProperty.Register("ContentOn", typeof(object),
        typeof(Switch), new UIPropertyMetadata(null));

    public object ContentOff
    {
      get { return (object)GetValue(ContentOffProperty); }
      set { SetValue(ContentOffProperty, value); }
    }

    public static readonly DependencyProperty ContentOffProperty =
        DependencyProperty.Register("ContentOff", typeof(object),
        typeof(Switch), new UIPropertyMetadata(null));

    #endregion
  }

  public class BreadcrumbBase : ItemsControl
  {
    #region Constructor

    static BreadcrumbBase()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(BreadcrumbBase),
          new FrameworkPropertyMetadata(typeof(BreadcrumbBase)));
    }

    #endregion

    #region Methods

    public virtual void Select(object value)
    {
      SelectedValue = value;
    }


    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      bcore = this.Template.FindName("PART_BreadcrumbCore", this) as BreadcrumbCore;
      tbox = this.Template.FindName("PART_TextBox", this) as SuggestBoxBase;
      toggle = this.Template.FindName("PART_Toggle", this) as ToggleButton;

      #region BreadcrumbCore related handlers
      //When Breadcrumb select a value, update it.
      AddHandler(BreadcrumbCore.SelectedValueChangedEvent, (RoutedEventHandler)((o, e) =>
      {
        Select(bcore.SelectedValue);
      }));
      #endregion

      #region SuggestBox related handlers.
      //When click empty space, switch to text box
      AddHandler(Breadcrumb.MouseDownEvent, (RoutedEventHandler)((o, e) =>
      {
        toggle.SetValue(ToggleButton.IsCheckedProperty, false); //Hide Breadcrumb
      }));

      //When text box is visible, call SelectAll
      toggle.AddValueChanged(ToggleButton.IsCheckedProperty,
          (o, e) =>
          {
            tbox.Focus();
            tbox.SelectAll();
          });



      //When changed selected (path) value, hide textbox.
      AddHandler(SuggestBox.ValueChangedEvent, (RoutedEventHandler)((o, e) =>
      {
        toggle.SetValue(ToggleButton.IsCheckedProperty, true); //Show Breadcrumb
      }));
      this.AddValueChanged(Breadcrumb.SelectedPathValueProperty, (o, e) =>
      {
        toggle.SetValue(ToggleButton.IsCheckedProperty, true); //Show Breadcrumb
      });
      this.AddValueChanged(Breadcrumb.SelectedValueProperty, (o, e) =>
      {
        toggle.SetValue(ToggleButton.IsCheckedProperty, true); //Show Breadcrumb
      });

      #endregion

    }

    public static void OnSelectedValueChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      var bread = sender as BreadcrumbBase;
      if (bread.bcore != null && (e.NewValue == null || !e.NewValue.Equals(e.OldValue)))
        bread.Select(e.NewValue);
    }




    #endregion

    #region Data

    protected BreadcrumbCore bcore;
    protected SuggestBoxBase tbox;
    protected ToggleButton toggle;

    #endregion

    #region Public Properties

    public SuggestBoxBase PART_SuggestBox { get { return tbox; } }
    public BreadcrumbCore PART_BreadcrumbCore { get { return bcore; } }
    public ToggleButton PART_Toggle { get { return toggle; } }

    /// <summary>
    /// Selected value object, it's path is retrieved from HierarchyHelper.GetPath(), not bindable at this time
    /// </summary>
    public object SelectedValue
    {
      get { return GetValue(SelectedValueProperty); }
      set { SetValue(SelectedValueProperty, value); }
    }

    public static readonly DependencyProperty SelectedValueProperty =
        DependencyProperty.Register("SelectedValue", typeof(object),
        typeof(BreadcrumbBase), new UIPropertyMetadata(null, OnSelectedValueChanged));


    #region ProgressBar related - IsIndeterminate, IsProgressbarVisible, ProgressBarValue
    /// <summary>
    /// Toggle whether the progress bar is indertminate
    /// </summary>
    public bool IsIndeterminate
    {
      get { return (bool)GetValue(IsIndeterminateProperty); }
      set { SetValue(IsIndeterminateProperty, value); }
    }

    public static readonly DependencyProperty IsIndeterminateProperty =
        DependencyProperty.Register("IsIndeterminate", typeof(bool),
        typeof(BreadcrumbBase), new UIPropertyMetadata(true));

    /// <summary>
    /// Toggle whether Progressbar visible
    /// </summary>
    public bool IsProgressbarVisible
    {
      get { return (bool)GetValue(IsProgressbarVisibleProperty); }
      set { SetValue(IsProgressbarVisibleProperty, value); }
    }

    public static readonly DependencyProperty IsProgressbarVisibleProperty =
        DependencyProperty.Register("IsProgressbarVisible", typeof(bool),
        typeof(BreadcrumbBase), new UIPropertyMetadata(false));

    /// <summary>
    /// Value of Progressbar.
    /// </summary>
    public int Progress
    {
      get { return (int)GetValue(ProgressProperty); }
      set { SetValue(ProgressProperty, value); }
    }

    public static readonly DependencyProperty ProgressProperty =
        DependencyProperty.Register("Progress", typeof(int),
        typeof(BreadcrumbBase), new UIPropertyMetadata(0));

    #endregion

    #region IsBreadcrumbVisible, DropDownHeight, DropDownWidth

    /// <summary>
    /// Toggle whether Breadcrumb (or SuggestBox) visible
    /// </summary>
    public bool IsBreadcrumbVisible
    {
      get { return (bool)GetValue(IsBreadcrumbVisibleProperty); }
      set { SetValue(IsBreadcrumbVisibleProperty, value); }
    }

    public static readonly DependencyProperty IsBreadcrumbVisibleProperty =
        DependencyProperty.Register("IsBreadcrumbVisible", typeof(bool),
        typeof(BreadcrumbBase), new UIPropertyMetadata(true));

    public static readonly DependencyProperty DropDownHeightProperty =
          BreadcrumbCore.DropDownHeightProperty.AddOwner(typeof(BreadcrumbBase));

    /// <summary>
    /// Is current dropdown (combobox) opened, this apply to the first &lt;&lt; button only
    /// </summary>
    public double DropDownHeight
    {
      get { return (double)GetValue(DropDownHeightProperty); }
      set { SetValue(DropDownHeightProperty, value); }
    }

    public static readonly DependencyProperty DropDownWidthProperty =
        BreadcrumbCore.DropDownWidthProperty.AddOwner(typeof(BreadcrumbBase));

    /// <summary>
    /// Is current dropdown (combobox) opened, this apply to the first &lt;&lt; button only
    /// </summary>
    public double DropDownWidth
    {
      get { return (double)GetValue(DropDownWidthProperty); }
      set { SetValue(DropDownWidthProperty, value); }
    }

    #endregion

    #region Header/Icon Template
    public static readonly DependencyProperty HeaderTemplateProperty =
                BreadcrumbCore.HeaderTemplateProperty.AddOwner(typeof(BreadcrumbBase));

    /// <summary>
    /// DataTemplate define the header text, (see also IconTemplate)
    /// </summary>
    public DataTemplate HeaderTemplate
    {
      get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
      set { SetValue(HeaderTemplateProperty, value); }
    }

    public static readonly DependencyProperty IconTemplateProperty =
       DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(BreadcrumbBase), new PropertyMetadata(null));

    /// <summary>
    /// DataTemplate define the icon.
    /// </summary>
    public DataTemplate IconTemplate
    {
      get { return (DataTemplate)GetValue(IconTemplateProperty); }
      set { SetValue(IconTemplateProperty, value); }
    }

    #endregion

    public static readonly DependencyProperty RootItemsSourceProperty =
     BreadcrumbCore.RootItemsSourceProperty.AddOwner(typeof(BreadcrumbBase));

    /// <summary>
    /// RootItemsSource - Items to be shown in BreadcrumbCore.
    /// ItemsSource - The Hierarchy for of current selected item.
    /// </summary>
    public IEnumerable RootItemsSource
    {
      get { return (IEnumerable)GetValue(RootItemsSourceProperty); }
      set { SetValue(RootItemsSourceProperty, value); }
    }


    public static readonly DependencyProperty ValuePathProperty =
       DependencyProperty.Register("ValuePath", typeof(string), typeof(BreadcrumbBase),
       new PropertyMetadata("Value"));

    /// <summary>
    /// Used by suggest box to obtain value
    /// </summary>
    public string ValuePath
    {
      get { return (string)GetValue(ValuePathProperty); }
      set { SetValue(ValuePathProperty, value); }
    }

    public static readonly DependencyProperty SuggestionsProperty =
        SuggestBox.SuggestionsProperty.AddOwner(typeof(BreadcrumbBase));

    /// <summary>
    /// Suggestions shown on the SuggestionBox
    /// </summary>
    public IList<object> Suggestions
    {
      get { return (IList<object>)GetValue(SuggestionsProperty); }
      set { SetValue(SuggestionsProperty, value); }
    }


    public static readonly DependencyProperty TextProperty =
    SuggestBox.TextProperty.AddOwner(typeof(BreadcrumbBase));

    /// <summary>
    /// Text shown on the SuggestionBox
    /// </summary>
    public string Text
    {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }





    public static readonly DependencyProperty ButtonsProperty =
        DependencyProperty.Register("Buttons", typeof(object), typeof(BreadcrumbBase));

    /// <summary>
    /// Buttons shown in the right side of the Breadcrumb
    /// </summary>
    public object Buttons
    {
      get { return GetValue(ButtonsProperty); }
      set { SetValue(ButtonsProperty, value); }
    }


    #endregion
  }
}
