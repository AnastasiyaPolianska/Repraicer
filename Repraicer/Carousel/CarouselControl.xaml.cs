using Carousel.Enums;
using Carousel.Helpers;
using Expression.Samples.PathListBoxUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Carousel
{
    /// <summary>
    /// The Carousel itself is based on PathListBox and PathListBoxUtils 
    /// using the codeplex available PathListBoxutils
    /// http://expressionblend.codeplex.com/wikipage?title=CarouselWPFTestAppUtils&referringTitle=Documentation
    /// 
    /// This article is also useful to see how the basic Carousel is made
    /// http://www.microsoft.com/design/toolbox/tutorials/CarouselWPFTestApp/carousel.aspx
    /// </summary>
    public partial class CarouselControl
    {
        #region Data

        private readonly List<PathListBoxItemTransformer> _transformers = new List<PathListBoxItemTransformer>();

        private PathListBoxScrollBehavior _pathListBoxScrollBehavior;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CarouselControl"/> class.
        /// </summary>
        public CarouselControl()
        {
            InitializeComponent();

            Loaded += CarouselControl_Loaded;
            pathListBox.SelectionChanged += PathListBox_SelectionChanged;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether [is valid number of items on path] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is valid number of items on path] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValidNumberOfItemsOnPath(object value)
        {
            var v = (int)value;

            return (!v.Equals(int.MinValue) &&
                !v.Equals(int.MaxValue));
        }

        /// <summary>
        /// Sets the visibility for path.
        /// </summary>
        /// <param name="pathType">Type of the path.</param>
        private void SetVisibilityForPath(PathType pathType)
        {
            foreach (UIElement uiElement in gridForKnownPaths.Children)
            {
                uiElement.Visibility = Visibility.Collapsed;
            }

            switch (pathType)
            {
                case PathType.Ellipse:
                    {
                        ellipsePath.Visibility = Visibility.Visible;
                        pathListBox.LayoutPaths[0].SourceElement = ellipsePath;
                    }
                    break;

                case PathType.Wave:
                    {
                        wavePath.Visibility = Visibility.Visible;
                        pathListBox.LayoutPaths[0].SourceElement = wavePath;
                    }
                    break;

                case PathType.Diagonal:
                    {
                        diagonalPath.Visibility = Visibility.Visible;
                        pathListBox.LayoutPaths[0].SourceElement = diagonalPath;
                    }
                    break;

                case PathType.ZigZag:
                    {
                        zigzagPath.Visibility = Visibility.Visible;
                        pathListBox.LayoutPaths[0].SourceElement = zigzagPath;
                    }
                    break;

                case PathType.Custom:
                    {
                        pathListBox.LayoutPaths[0].SourceElement = CustomPathElement;
                    }
                    break;

                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(pathType), pathType, null);
                    }
            }
        }

        /// <summary>
        /// Handles the Loaded event of the CarouselControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CarouselControl_Loaded(object sender, RoutedEventArgs e)
        {
            var behaviors = Interaction.GetBehaviors(pathListBox);

            _pathListBoxScrollBehavior = (PathListBoxScrollBehavior)behaviors
                .FirstOrDefault(x => x.GetType() == typeof(PathListBoxScrollBehavior));

            pathListBox.SelectedIndex = 3;
        }

        /// <summary>
        /// Handles the SelectionChanged event of the PathListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void PathListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => RaiseSelectionChangedEvent(e);

        /// <summary>
        /// Handles the Loaded event of the PathListBoxItemTransformer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PathListBoxItemTransformer_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is PathListBoxItemTransformer trans)) return;

            trans.Ease = AnimationEaseIn;
            trans.OpacityRange = OpacityRange;
            trans.ScaleRange = ScaleRange;
            trans.AngleRange = AngleRange;
            _transformers.Add(trans);
        }

        /// <summary>
        /// The pathListBox transitioning did not seem to work when I used an ICommand
        /// could be it was firing to often and causing some mischief to happen
        /// </summary>
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (pathListBox.SelectedIndex > 0)
            {
                pathListBox.SelectedIndex = pathListBox.SelectedIndex - 1;
            }
            else
            {
                pathListBox.SelectedIndex = pathListBox.Items.Count - 1;
            }

            pathListBox.SelectedItem = pathListBox.Items[pathListBox.SelectedIndex];
        }

        /// <summary>
        /// The pathListBox transitioning did not seem to work when I used an ICommand
        /// could be it was firing to often and causing some mischief to happen
        /// </summary>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (pathListBox.SelectedIndex < pathListBox.Items.Count - 1)
            {
                pathListBox.SelectedIndex = pathListBox.SelectedIndex + 1;
            }
            else
            {
                pathListBox.SelectedIndex = 0;
            }

            pathListBox.SelectedItem = pathListBox.Items[pathListBox.SelectedIndex];
        }

        #endregion

        #region Events

        #region SelectionChanged

        /// <summary>
        /// SelectionChanged Routed Event, which is raised when
        /// the internal PathListBox.SelectionChanged event occurs
        /// </summary>
        public static readonly RoutedEvent SelectionChangedEvent =
            EventManager.RegisterRoutedEvent(
                "SelectionChanged",
                RoutingStrategy.Bubble,
                typeof(SelectionChangedEventHandler),
                typeof(CarouselControl));

        /// <summary>
        /// Occurs when [selection changed].
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged
        {
            add => AddHandler(SelectionChangedEvent, value);
            remove => RemoveHandler(SelectionChangedEvent, value);
        }

        /// <summary>
        /// Raises the selection changed event.
        /// </summary>
        /// <param name="arg">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        private void RaiseSelectionChangedEvent(RoutedEventArgs arg)
        {
            arg.RoutedEvent = SelectionChangedEvent;
            RoutedEventHelper.RaiseEvent(this, arg);
        }

        #endregion

        #endregion

        #region DPs

        #region CustomPathElement

        /// <summary>
        /// The FrameworkElement to use as custom path for Carousel
        /// </summary>
        public static readonly DependencyProperty CustomPathElementProperty =
            DependencyProperty.Register(
                "CustomPathElement",
                typeof(FrameworkElement),
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    null,
                    OnCustomPathElementChanged));

        /// <summary>
        /// Gets or sets the custom path element.
        /// </summary>
        /// <value>
        /// The custom path element.
        /// </value>
        public FrameworkElement CustomPathElement
        {
            get => (FrameworkElement)GetValue(CustomPathElementProperty);
            set => SetValue(CustomPathElementProperty, value);
        }

        /// <summary>
        /// Called when [custom path element changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnCustomPathElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnCustomPathElementChanged(e);

        /// <summary>
        /// Raises the <see cref="E:CustomPathElementChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnCustomPathElementChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                SetVisibilityForPath(PathType.Custom);
            }
        }

        #endregion

        #region NavigationButtonPosition

        /// <summary>
        /// The Navigation ButtonPosition to use for the Carousel
        /// </summary>
        public static readonly DependencyProperty NavigationButtonPositionProperty =
            DependencyProperty.Register(
                "NavigationButtonPosition", 
                typeof(ButtonPosition), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    ButtonPosition.BottomCenter,
                    OnNavigationButtonPositionChanged));

        /// <summary>
        /// Gets or sets the navigation button position.
        /// </summary>
        /// <value>
        /// The navigation button position.
        /// </value>
        public ButtonPosition NavigationButtonPosition
        {
            get => (ButtonPosition)GetValue(NavigationButtonPositionProperty);
            set => SetValue(NavigationButtonPositionProperty, value);
        }

        /// <summary>
        /// Called when [navigation button position changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnNavigationButtonPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnNavigationButtonPositionChanged(e);

        /// <summary>
        /// Raises the <see cref="E:NavigationButtonPositionChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnNavigationButtonPositionChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region PreviousButtonStyle

        /// <summary>
        /// Can be used to specify a new Style for Previous button to use for the Carousel
        /// </summary>
        public static readonly DependencyProperty PreviousButtonStyleProperty =
            DependencyProperty.Register(
                "PreviousButtonStyle", 
                typeof(Style), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    null,
                    OnPreviousButtonStyleChanged));

        /// <summary>
        /// Gets or sets the previous button style.
        /// </summary>
        /// <value>
        /// The previous button style.
        /// </value>
        public Style PreviousButtonStyle
        {
            get => (Style)GetValue(PreviousButtonStyleProperty);
            set => SetValue(PreviousButtonStyleProperty, value);
        }

        /// <summary>
        /// Called when [previous button style changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnPreviousButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnPreviousButtonStyleChanged(e);

        /// <summary>
        /// Raises the <see cref="E:PreviousButtonStyleChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnPreviousButtonStyleChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                previousButton.Style = (Style)e.NewValue;
            }
        }

        #endregion

        #region NextButtonStyle

        /// <summary>
        /// Can be used to specify a new Style for Next button to use for the Carousel
        /// </summary>
        public static readonly DependencyProperty NextButtonStyleProperty =
            DependencyProperty.Register(
                "NextButtonStyle", 
                typeof(Style), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    null,
                    OnNextButtonStyleChanged));

        /// <summary>
        /// Gets or sets the next button style.
        /// </summary>
        /// <value>
        /// The next button style.
        /// </value>
        public Style NextButtonStyle
        {
            get => (Style)GetValue(NextButtonStyleProperty);
            set => SetValue(NextButtonStyleProperty, value);
        }

        /// <summary>
        /// Called when [next button style changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnNextButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnNextButtonStyleChanged(e);

        /// <summary>
        /// Raises the <see cref="E:NextButtonStyleChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnNextButtonStyleChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                nextButton.Style = (Style)e.NewValue;
            }
        }

        #endregion

        #region PathType

        /// <summary>
        /// PathType Dependency Property
        /// </summary>
        public static readonly DependencyProperty PathTypeProperty =
            DependencyProperty.Register(
                "PathType", 
                typeof(PathType), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    PathType.Custom,
                    OnPathTypeChanged));

        /// <summary>
        /// Gets or sets the type of the path.
        /// </summary>
        /// <value>
        /// The type of the path.
        /// </value>
        public PathType PathType
        {
            get => (PathType)GetValue(PathTypeProperty);
            set => SetValue(PathTypeProperty, value);
        }

        /// <summary>
        /// Called when [path type changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnPathTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnPathTypeChanged(e);

        /// <summary>
        /// Raises the <see cref="E:PathTypeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnPathTypeChanged(DependencyPropertyChangedEventArgs e) => SetVisibilityForPath((PathType)e.NewValue);

        #endregion

        #region AnimationEaseIn

        /// <summary>
        /// AnimationEaseIn Dependency Property
        /// </summary>
        public static readonly DependencyProperty AnimationEaseInProperty =
            DependencyProperty.Register(
                "AnimationEaseIn",
                typeof(EasingFunctionBase), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    null,
                    OnAnimationEaseInChanged));

        /// <summary>
        /// Gets or sets the animation ease in.
        /// </summary>
        /// <value>
        /// The animation ease in.
        /// </value>
        public EasingFunctionBase AnimationEaseIn
        {
            get => (EasingFunctionBase)GetValue(AnimationEaseInProperty);
            set => SetValue(AnimationEaseInProperty, value);
        }

        /// <summary>
        /// Called when [animation ease in changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnAnimationEaseInChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnAnimationEaseInChanged(e);

        /// <summary>
        /// Raises the <see cref="E:AnimationEaseInChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnAnimationEaseInChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            foreach (var pathListBoxItemTransformer in _transformers)
            {
                pathListBoxItemTransformer.Ease = (EasingFunctionBase)e.NewValue;
            }
        }

        #endregion

        #region AnimationEaseOut

        /// <summary>
        /// AnimationEaseOut Dependency Property
        /// </summary>
        public static readonly DependencyProperty AnimationEaseOutProperty =
            DependencyProperty.Register(
                "AnimationEaseOut", 
                typeof(EasingFunctionBase), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    null,
                    OnAnimationEaseOutChanged));

        /// <summary>
        /// Gets or sets the animation ease out.
        /// </summary>
        /// <value>
        /// The animation ease out.
        /// </value>
        public EasingFunctionBase AnimationEaseOut
        {
            get => (EasingFunctionBase)GetValue(AnimationEaseOutProperty);
            set => SetValue(AnimationEaseOutProperty, value);
        }

        /// <summary>
        /// Called when [animation ease out changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnAnimationEaseOutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnAnimationEaseOutChanged(e);

        /// <summary>
        /// Raises the <see cref="E:AnimationEaseOutChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnAnimationEaseOutChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            if (_pathListBoxScrollBehavior != null)
            {
                _pathListBoxScrollBehavior.Ease = (EasingFunctionBase)e.NewValue;
            }
        }

        #endregion

        #region OpacityRange

        /// <summary>
        /// OpacityRange to use for PathListBoxItemTransformer
        /// </summary>
        public static readonly DependencyProperty OpacityRangeProperty =
            DependencyProperty.Register(
                "OpacityRange", 
                typeof(Point), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    new Point(0.7, 1.0),
                    OnOpacityRangeChanged));

        /// <summary>
        /// Gets or sets the opacity range.
        /// </summary>
        /// <value>
        /// The opacity range.
        /// </value>
        public Point OpacityRange
        {
            get => (Point)GetValue(OpacityRangeProperty);
            set => SetValue(OpacityRangeProperty, value);
        }

        /// <summary>
        /// Handles changes to the OpacityRange property.
        /// </summary>
        private static void OnOpacityRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnOpacityRangeChanged(e);

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the OpacityRange property.
        /// </summary>
        protected virtual void OnOpacityRangeChanged(DependencyPropertyChangedEventArgs e)
        {
            foreach (var pathListBoxItemTransformer in _transformers)
            {
                pathListBoxItemTransformer.OpacityRange = (Point)e.NewValue;
            }
        }

        #endregion

        #region ScaleRange

        /// <summary>
        /// ScaleRange to use for PathListBoxItemTransformer
        /// </summary>
        public static readonly DependencyProperty ScaleRangeProperty =
            DependencyProperty.Register(
                "ScaleRange", 
                typeof(Point), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    new Point(1.0, 3.0),
                    OnScaleRangeChanged));

        /// <summary>
        /// Gets or sets the scale range.
        /// </summary>
        /// <value>
        /// The scale range.
        /// </value>
        public Point ScaleRange
        {
            get => (Point)GetValue(ScaleRangeProperty);
            set => SetValue(ScaleRangeProperty, value);
        }

        /// <summary>
        /// Handles changes to the ScaleRange property.
        /// </summary>
        private static void OnScaleRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnScaleRangeChanged(e);

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ScaleRange property.
        /// </summary>
        protected virtual void OnScaleRangeChanged(DependencyPropertyChangedEventArgs e)
        {
            foreach (var pathListBoxItemTransformer in _transformers)
            {
                pathListBoxItemTransformer.ScaleRange = (Point)e.NewValue;
            }
        }

        #endregion

        #region AngleRange

        /// <summary>
        /// AngleRange to use for PathListBoxItemTransformer
        /// </summary>
        public static readonly DependencyProperty AngleRangeProperty =
            DependencyProperty.Register(
                "AngleRange", 
                typeof(Point), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    new Point(0.0, 0.0),
                    OnAngleRangeChanged));

        /// <summary>
        /// Gets or sets the angle range.
        /// </summary>
        /// <value>
        /// The angle range.
        /// </value>
        public Point AngleRange
        {
            get => (Point)GetValue(AngleRangeProperty);
            set => SetValue(AngleRangeProperty, value);
        }

        /// <summary>
        /// Handles changes to the AngleRange property.
        /// </summary>
        private static void OnAngleRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnAngleRangeChanged(e);

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the AngleRange property.
        /// </summary>
        protected virtual void OnAngleRangeChanged(DependencyPropertyChangedEventArgs e)
        {
            foreach (var pathListBoxItemTransformer in _transformers)
            {
                pathListBoxItemTransformer.AngleRange = (Point)e.NewValue;
            }
        }

        #endregion

        //Path ListBox wrapped properties

        #region DataTemplateToUse

        /// <summary>
        /// The DataTemplate to use for the Carousel
        /// </summary>
        [Bindable(true)]
        public static readonly DependencyProperty DataTemplateToUseProperty =
            DependencyProperty.Register(
                "DataTemplateToUse", 
                typeof(DataTemplate), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    null,
                    OnDataTemplateToUseChanged));

        /// <summary>
        /// Gets or sets the data template to use.
        /// </summary>
        /// <value>
        /// The data template to use.
        /// </value>
        public DataTemplate DataTemplateToUse
        {
            get => (DataTemplate)GetValue(DataTemplateToUseProperty);
            set => SetValue(DataTemplateToUseProperty, value);
        }

        /// <summary>
        /// Called when [data template to use changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnDataTemplateToUseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnDataTemplateToUseChanged(e);

        /// <summary>
        /// Raises the <see cref="E:DataTemplateToUseChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnDataTemplateToUseChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                pathListBox.ItemTemplate = (DataTemplate)e.NewValue;
            }
        }

        #endregion

        #region SelectedItem

        /// <summary>
        /// SelectedItem Dependency Property
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem", 
                typeof(object), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    null,
                    OnSelectedItemChanged));

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public object SelectedItem
        {
            get => pathListBox.GetValue(SelectedItemProperty);
            set => pathListBox.SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// Called when [selected item changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnSelectedItemChanged(e);

        /// <summary>
        /// Raises the <see cref="E:SelectedItemChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnSelectedItemChanged(DependencyPropertyChangedEventArgs e)
        {
            var currentlySelected = pathListBox.SelectedItem;

            if (currentlySelected != e.NewValue)
            {
                pathListBox.SelectedItem = e.NewValue;
            }
        }

        #endregion

        #region ItemsSource

        /// <summary>
        /// The ItemsSource to use for the Carousel
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource", 
                typeof(IEnumerable), 
                typeof(CarouselControl),
                new FrameworkPropertyMetadata(
                    null,
                    OnItemsSourceChanged));

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// Called when [items source changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CarouselControl)d).OnItemsSourceChanged(e);

        /// <summary>
        /// Raises the <see cref="E:ItemsSourceChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnItemsSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            _transformers.Clear();
            pathListBox.ItemsSource = (IEnumerable)e.NewValue;
        }

        #endregion

        #region NumberOfItemsOnPath

        /// <summary>
        /// NumberOfItemsOnPath
        /// </summary>
        public static readonly DependencyProperty NumberOfItemsOnPathProperty = DependencyProperty.Register(
            "NumberOfItemsOnPath",
            typeof(int),
            typeof(CarouselControl),
            new FrameworkPropertyMetadata(
                7,
                FrameworkPropertyMetadataOptions.None,
                OnNumberOfItemsOnPathChanged,
                CoerceNumberOfItemsOnPath
            ),
            IsValidNumberOfItemsOnPath
        );

        //property accessors
        public int NumberOfItemsOnPath
        {
            get => (int)GetValue(NumberOfItemsOnPathProperty);
            set => SetValue(NumberOfItemsOnPathProperty, value);
        }

        /// <summary>
        /// Coerce NumberOfItemsOnPath value if not within limits
        /// </summary>
        private static object CoerceNumberOfItemsOnPath(DependencyObject d, object value)
        {
            var depObj = (CarouselControl)d;
            var current = (int)value;

            if (current < depObj.MinNumberOfItemsOnPath)
            {
                current = depObj.MinNumberOfItemsOnPath;
            }

            if (current > depObj.MaxNumberOfItemsOnPath)
            {
                current = depObj.MaxNumberOfItemsOnPath;
            }

            return current;
        }

        /// <summary>
        /// Called when [number of items on path changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnNumberOfItemsOnPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(MinNumberOfItemsOnPathProperty);  //invokes the CoerceValueCallback delegate ("CoerceMinNumberOfItemsOnPath")
            d.CoerceValue(MaxNumberOfItemsOnPathProperty);  //invokes the CoerceValueCallback delegate ("CoerceMaxNumberOfItemsOnPath")

            var depObj = (CarouselControl)d;

            depObj.pathListBox.LayoutPaths[0].Capacity = depObj.NumberOfItemsOnPath;
        }

        #endregion

        #region MinNumberOfItemsOnPath

        /// <summary>
        /// MinNumberOfItemsOnPath DP
        /// </summary>
        public static readonly DependencyProperty MinNumberOfItemsOnPathProperty = DependencyProperty.Register(
        "MinNumberOfItemsOnPath",
        typeof(int),
        typeof(CarouselControl),
        new FrameworkPropertyMetadata(
            3,
            FrameworkPropertyMetadataOptions.None,
            OnMinNumberOfItemsOnPathChanged,
            CoerceMinNumberOfItemsOnPath
        ),
        IsValidNumberOfItemsOnPath);

        /// <summary>
        /// Gets or sets the minimum number of items on path.
        /// </summary>
        /// <value>
        /// The minimum number of items on path.
        /// </value>
        public int MinNumberOfItemsOnPath
        {
            get => (int)GetValue(MinNumberOfItemsOnPathProperty);
            set => SetValue(MinNumberOfItemsOnPathProperty, value);
        }

        /// <summary>
        /// Coerce MinNumberOfItemsOnPath value if not within limits
        /// </summary>
        private static void OnMinNumberOfItemsOnPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(MaxNumberOfItemsOnPathProperty);  //invokes the CoerceValueCallback delegate ("CoerceMaxNumberOfItemsOnPath")
            d.CoerceValue(NumberOfItemsOnPathProperty);  //invokes the CoerceValueCallback delegate ("CoerceNumberOfItemsOnPath")
        }

        /// <summary>
        /// Coerces the minimum number of items on path.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static object CoerceMinNumberOfItemsOnPath(DependencyObject d, object value)
        {
            var depObj = (CarouselControl)d;
            var min = (int)value;

            if (min > depObj.MaxNumberOfItemsOnPath)
            {
                min = depObj.MaxNumberOfItemsOnPath;
            }

            return min;
        }
        #endregion

        #region MaxNumberOfItemsOnPath

        /// <summary>
        /// MaxNumberOfItemsOnPath
        /// </summary>
        public static readonly DependencyProperty MaxNumberOfItemsOnPathProperty = DependencyProperty.Register(
            "MaxNumberOfItemsOnPath",
            typeof(int),
            typeof(CarouselControl),
            new FrameworkPropertyMetadata(
                10,
                FrameworkPropertyMetadataOptions.None,
                OnMaxNumberOfItemsOnPathChanged,
                CoerceMaxNumberOfItemsOnPath
            ),
            IsValidNumberOfItemsOnPath
        );

        /// <summary>
        /// Gets or sets the maximum number of items on path.
        /// </summary>
        /// <value>
        /// The maximum number of items on path.
        /// </value>
        public int MaxNumberOfItemsOnPath
        {
            get => (int)GetValue(MaxNumberOfItemsOnPathProperty);
            set => SetValue(MaxNumberOfItemsOnPathProperty, value);
        }

        /// <summary>
        /// Coerce MaxNumberOfItemsOnPath value if not within limits
        /// </summary>
        private static object CoerceMaxNumberOfItemsOnPath(DependencyObject d, object value)
        {
            var depObj = (CarouselControl)d;
            var max = (int)value;

            if (max < depObj.MinNumberOfItemsOnPath)
            {
                max = depObj.MinNumberOfItemsOnPath;
            }

            return max;
        }

        /// <summary>
        /// Called when [maximum number of items on path changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnMaxNumberOfItemsOnPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(MinNumberOfItemsOnPathProperty);  //invokes the CoerceValueCallback delegate ("CoerceMinNumberOfItemsOnPath")
            d.CoerceValue(NumberOfItemsOnPathProperty);  //invokes the CoerceValueCallback delegate ("CoerceNumberOfItemsOnPath")
        }

        #endregion

        #endregion
    }
}