using Newtonsoft.Json;
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
    public sealed partial class MultiEdit : Page
    {
        IList<Light> lights = new List<Light>();
        MainPage main;
        int hue = 65535;
        int sat = 254;
        int bri = 254;

        public MultiEdit()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            main = e.Parameter as MainPage;
            foreach(var v in main.GetListView().SelectedItems)
            {
                Light l = (Light)v;
                lights.Add(l);
            }
            this.DataContext = this;
        }


        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            //l.UpdateColor();
        }

        private void Slider_Released(object sender, PointerRoutedEventArgs e)
        {
            foreach(var l in lights)
            {
                l.Hue = HueSlider.Value;
                l.Saturation = SaturationSlider.Value;
                l.Brightness = BrightnessSlider.Value;
                ColorRectangle.Fill = new SolidColorBrush(ColorUtil.getColor(l));
                l.UpdateColor();
            }
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch button = ((ToggleSwitch)sender);
            foreach(var l in lights)
            {
                
                l.UpdateState(button.IsOn);
            }
                
        }
    }
}
