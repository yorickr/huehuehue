using HueUWP.Helpers;
using HueUWP.Lights;
using HueUWP.Views;
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
        LightsView main;

        public MultiEdit()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            main = e.Parameter as LightsView;

            HueSlider.Value = 0;
            SaturationSlider.Value = 0;
            BrightnessSlider.Value = 0;
            OnOffButton.IsOn = false;

            //foreach (var v in main.GetListView().SelectedItems)
            //{
            //    Light l = (Light)v;
            //    lights.Add(l);
            //}

            LightsSelectedField.Text = lights.ToDelimitedString(l => l.Name);
        }


        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            //l.UpdateColor();
        }

        private void Slider_Released(object sender, PointerRoutedEventArgs e)
        {
            foreach (var l in lights)
            {
                l.Hue = (int)HueSlider.Value;
                l.Saturation = (int)SaturationSlider.Value;
                l.Brightness = (int)BrightnessSlider.Value;
                ColorRectangle.Fill = new SolidColorBrush(ColorUtil.getColor(l));
                l.SetColor();
            }
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch button = ((ToggleSwitch)sender);
            foreach (var l in lights)
            {

                l.SetState();
            }

        }
    }
}
