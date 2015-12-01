using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HueUWP
{



    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static ApplicationDataContainer LOCAL_SETTINGS = ApplicationData.Current.LocalSettings;

        private ObservableCollection<Light> _lightsViewModel = LightDataSource.GetLights();

        public APIHandler api;

        public ObservableCollection<Light> LightsViewModel
        {
            get { return this._lightsViewModel; }
        }

        public MainPage()
        {
            this.InitializeComponent();
            //LOCAL_SETTINGS.Values["ip"] = "localhost";
            //LOCAL_SETTINGS.Values["port"] = 8000;
            NetworkHandler nwh = new NetworkHandler();
            api = new APIHandler(nwh);
            _lightsViewModel.Clear();
            api.GetAllLights(_lightsViewModel);
        }

        private void ColorChanged(object sender, RoutedEventArgs e)
        {
            Button button = ((Button)sender);
            Light light = (Light)button.DataContext;

            light.UpdateColor(10, 10, 10);
        }

        public void ToggleSwitch_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Hello again");
            ToggleSwitch button = ((ToggleSwitch)sender);
            Light light = (Light)button.DataContext;
            Debug.WriteLine("Hello");
            light.UpdateState(button.IsOn);
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch button = ((ToggleSwitch)sender);
            Light light = (Light)button.DataContext;
            light.UpdateState(button.IsOn);
            light.UpdateColor(0, 38, 255);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootframe = Window.Current.Content as Frame;
            rootframe.Navigate(typeof(SettingsView), api);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _lightsViewModel.Clear();
            api.GetAllLights(_lightsViewModel);
        }

        private void myListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Light l = e.ClickedItem as Light;
            Frame rootframe = Window.Current.Content as Frame;
            if (l != null)
                rootframe.Navigate(typeof(DetailView), l);
        }
    }
}
