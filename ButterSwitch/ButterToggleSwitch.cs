using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace ButterSwitch
{
    /// <summary>
    /// Represents a butter-look switch that can be toggled between two states.
    /// </summary>
    public sealed class ButterToggleSwitch : Control
    {
        #region PUBLIC PROPERTIES

        /// <summary>Gets or sets a value that declares whether the state of the ToggleSwitch is On.</summary>
        /// <returns>true if the state is On; false if the state is Off.</returns>
        public bool IsOn
        {
            get { return (bool)GetValue(IsOnProperty); }
            set { SetValue(IsOnProperty, value); }
        }

        /// <summary>Gets or sets a value that declares whether the state of the ToggleSwitch is On.</summary>
        /// <returns>true if the state is On; false if the state is Off.</returns>
        public static readonly DependencyProperty IsOnProperty =
            DependencyProperty.Register("IsOn", typeof(bool), typeof(ButterToggleSwitch),
                new PropertyMetadata(false, OnToggled));

        /// <summary>Identifies the IsEnabled dependency property.</summary>
        /// <returns>The identifier for the IsEnabled dependency property.</returns>
        public new bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        /// <summary>Identifies the IsEnabled dependency property.</summary>
        /// <returns>The identifier for the IsEnabled dependency property.</returns>
        public new static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(ButterToggleSwitch),
                new PropertyMetadata(true, OnIsEnabledChanged));

        /// <summary>Provides the object content that should be displayed using the OnContentTemplate when this ToggleSwitch has state of On.</summary>
        /// <returns>The object content. In some cases this is a string, in other cases it is a single element that provides a root for further composition content. Probably the most common set usage is to place a binding here.</returns>
        public object OnContent
        {
            get { return GetValue(OnContentProperty); }
            set { SetValue(OnContentProperty, value); }
        }

        /// <summary>Provides the object content that should be displayed using the OnContentTemplate when this ToggleSwitch has state of On.</summary>
        /// <returns>The object content. In some cases this is a string, in other cases it is a single element that provides a root for further composition content. Probably the most common set usage is to place a binding here.</returns>
        public static readonly DependencyProperty OnContentProperty =
            DependencyProperty.Register("OnContent", typeof(object), typeof(ButterToggleSwitch),
                new PropertyMetadata("On", null));

        /// <summary>Provides the object content that should be displayed using the OffContentTemplate when this ToggleSwitch has state of Off.</summary>
        /// <returns>The object content. In some cases this is a string, in other cases it is a single element that provides a root for further composition content. Probably the most common set usage is to place a binding here.</returns>
        public object OffContent
        {
            get { return GetValue(OffContentProperty); }
            set { SetValue(OffContentProperty, value); }
        }

        /// <summary>Provides the object content that should be displayed using the OffContentTemplate when this ToggleSwitch has state of Off.</summary>
        /// <returns>The object content. In some cases this is a string, in other cases it is a single element that provides a root for further composition content. Probably the most common set usage is to place a binding here.</returns>
        public static readonly DependencyProperty OffContentProperty =
            DependencyProperty.Register("OffContent", typeof(object), typeof(ButterToggleSwitch),
                new PropertyMetadata("Off", null));

        /// <summary>
        /// Gets or sets the margin of the content.
        /// </summary>
        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin of the content.
        /// </summary>
        public static readonly DependencyProperty ContentMarginProperty =
            DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(ButterToggleSwitch),
                new PropertyMetadata("ButterToggleSwitch", null));

        /// <summary>
        /// Gets or sets the style of the content.
        /// </summary>
        public Style ContentStyle
        {
            get { return (Style)GetValue(ContentStyleProperty); }
            set { SetValue(ContentStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style of the content.
        /// </summary>
        public static readonly DependencyProperty ContentStyleProperty =
            DependencyProperty.Register("ContentStyle", typeof(Style), typeof(ButterToggleSwitch),
                new PropertyMetadata(null, null));

        /// <summary>Gets or sets the header content.</summary>
        /// <returns>The header content for the ToggleSwitch.</returns>
        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        /// <summary>Gets or sets the header content.</summary>
        /// <returns>The header content for the ToggleSwitch.</returns>
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("HeaderContent", typeof(object), typeof(ButterToggleSwitch),
                new PropertyMetadata("Header", null));

        /// <summary>
        /// Gets or sets the margin of the header.
        /// </summary>
        public Thickness HeaderMargin
        {
            get { return (Thickness)GetValue(HeaderMarginProperty); }
            set { SetValue(HeaderMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin of the header.
        /// </summary>
        public static readonly DependencyProperty HeaderMarginProperty =
            DependencyProperty.Register("HeaderMargin", typeof(Thickness), typeof(ButterToggleSwitch),
                new PropertyMetadata("ButterToggleSwitch", null));

        /// <summary>
        /// Gets or sets the style of the header content.
        /// </summary>
        public Style HeaderStyle
        {
            get { return (Style)GetValue(HeaderStyleProperty); }
            set { SetValue(HeaderStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style of the header content.
        /// </summary>
        public static readonly DependencyProperty HeaderStyleProperty =
            DependencyProperty.Register("HeaderStyle", typeof(Style), typeof(ButterToggleSwitch),
                new PropertyMetadata(null, null));

        #endregion

        /// <summary>Occurs when On/Off state changes for this ToggleSwitch.</summary>
        public event RoutedEventHandler Toggled;

        /// <summary>Initializes a new instance of the ButterToggleSwitch class.</summary>
        public ButterToggleSwitch()
        {
            this.DefaultStyleKey = typeof(ButterToggleSwitch);

        }

        private VisualStateGroup _toggleStatesGroup;
        private Grid _grdGraphics;

        private bool _isAnimationOnRunning;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _grdGraphics = GetTemplateChild("GrdGraphics") as Grid;
            _toggleStatesGroup = GetTemplateChild("ToggleStates") as VisualStateGroup;

            if (_grdGraphics == null || _toggleStatesGroup == null)
                return;

            _toggleStatesGroup.CurrentStateChanged += _toggleStatesGroup_CurrentStateChanged;
            _grdGraphics.Tapped += _grdGraphics_Tapped;
            _grdGraphics.PointerEntered += _grdGraphics_PointerEntered;
            _grdGraphics.PointerExited += _grdGraphics_PointerExited;

            if (this.IsOn)
            {
                VisualStateManager.GoToState(this, "On", false);
                _isAnimationOnRunning = false;

            }
            else
            {
                VisualStateManager.GoToState(this, "Off", false);
                _isAnimationOnRunning = false;
            }

            Toggled?.Invoke(this, new RoutedEventArgs());
        }

        private void _grdGraphics_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled)
                return;

            if (this.IsOn)
            {
                VisualStateManager.GoToState(this, "On", true);
                _isAnimationOnRunning = false;
            }
            else
            {
                VisualStateManager.GoToState(this, "Off", true);
                _isAnimationOnRunning = false;
            }
        }

        private void _grdGraphics_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled)
                return;

            if (this.IsOn)
            {
                VisualStateManager.GoToState(this, "OnHover", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "OffHover", true);
            }
        }

        private void _toggleStatesGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            _isAnimationOnRunning = false;
            Toggled?.Invoke(this, new RoutedEventArgs());
        }

        private void _grdGraphics_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!IsEnabled)
                return;

            if (!_isAnimationOnRunning)
                IsOn = !IsOn;
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            if (e.NewValue == e.OldValue)
                return;

            ButterToggleSwitch instance = d as ButterToggleSwitch;

            if (instance == null)
                return;

            if (instance.IsEnabled)
            {
                if (instance.IsOn)
                {
                    VisualStateManager.GoToState(instance, "On", false);
                    instance._isAnimationOnRunning = false;

                }
                else
                {
                    VisualStateManager.GoToState(instance, "Off", false);
                    instance._isAnimationOnRunning = false;
                }
            }
            else
            {
                VisualStateManager.GoToState(instance, "Disabled", false);
                instance._isAnimationOnRunning = false;
            }
        }

        private static void OnToggled(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            if (e.NewValue == e.OldValue)
                return;

            ButterToggleSwitch instance = d as ButterToggleSwitch;

            if (instance == null)
                return;

            bool useAnimation = !DesignMode.DesignModeEnabled;

            if (instance.IsOn)
            {
                instance._isAnimationOnRunning = true;
                VisualStateManager.GoToState(instance, "On", useAnimation);

            }
            else
            {
                instance._isAnimationOnRunning = true;
                VisualStateManager.GoToState(instance, "Off", useAnimation);
            }
        }
    }
}
