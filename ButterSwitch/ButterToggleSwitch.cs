using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        private Storyboard _sbOn2Off, _sbOff2On;
        private Grid _grdGraphics;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _sbOn2Off = GetTemplateChild("SbOn2Off") as Storyboard;
            _sbOff2On = GetTemplateChild("SbOff2On") as Storyboard;
            _grdGraphics = GetTemplateChild("GrdGraphics") as Grid;

            _sbOn2Off.Completed += OnSbCompleted;
            _sbOff2On.Completed += OnSbCompleted;
            _grdGraphics.Tapped += _grdGraphics_Tapped;
        }

        private void _grdGraphics_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IsOn = !IsOn;
        }

        private void OnSbCompleted(object sender, object e)
        {
            Toggled?.Invoke(this, new RoutedEventArgs());
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

            if (instance.IsOn)
            {
                instance._sbOff2On.Begin();
            }
            else
            {
                instance._sbOn2Off.Begin();
            }
        }
    }
}
