using HueUWP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BindingToCommandsUWP
{



    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Light> _lightsViewModel = LightDataSource.GetLights();

        public ObservableCollection<Light> LightsViewModel
        {
            get { return this._lightsViewModel; }
        }

        public MainPage()
        {
            this.InitializeComponent();
            Debug.WriteLine(new NetworkHandler().RegisterName("Kaas", "Henk"));
            Debug.WriteLine(new NetworkHandler().Test());
        }

        private void ColorChanged(object sender, RoutedEventArgs e)
        {
            Button button = ((Button)sender);
            Light light = (Light)button.DataContext;

            light.UpdateColor(10, 10, 10);
        }

        private void ToggleSwitch_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ToggleSwitch button = ((ToggleSwitch)sender);
            Light light = (Light)button.DataContext;

            light.UpdateState(button.IsOn);
        }
    }
}
