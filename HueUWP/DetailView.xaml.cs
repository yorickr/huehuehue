using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HueUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailView : Page
    {
        Light l;

        public DetailView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            l = e.Parameter as Light;
            this.DataContext = l;
        }


        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            //l.UpdateColor();
        }

        private void Slider_Released(object sender, PointerRoutedEventArgs e)
        {
            l.UpdateColor();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch button = ((ToggleSwitch)sender);
            Light light = (Light)button.DataContext;
            if (light != null)
                light.UpdateState(button.IsOn);
        }
    }
}
