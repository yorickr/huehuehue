using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Load_Lights();
        }

        private async void Load_Lights()
        {
            ErrorMessage.Text = "";
            FeedbackPanel.Visibility = Visibility.Visible;
            _lightsViewModel.Clear();

            Loading.IsActive = true;
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            string s = await api.GetAllLights(_lightsViewModel);

            if (s == "error")
                ErrorMessage.Text = "There was a problem connecting...";
            else if (_lightsViewModel.Count < 1)
                ErrorMessage.Text = "No lights found...";
            else
                FeedbackPanel.Visibility = Visibility.Collapsed;

            Loading.IsActive = false;
        }

        public void ToggleSwitch_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Hello again");
            ToggleSwitch button = ((ToggleSwitch)sender);
            Light light = (Light)button.DataContext;
            Debug.WriteLine("Hello");
            if(light != null)
                light.UpdateState(button.IsOn);
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch button = ((ToggleSwitch)sender);
            Light light = (Light)button.DataContext;
            if(light != null)
                light.UpdateState(button.IsOn);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootframe = Window.Current.Content as Frame;
            rootframe.Navigate(typeof(SettingsView), api);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Load_Lights();
        }

        private void myListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Light l = e.ClickedItem as Light;
            Frame rootframe = Window.Current.Content as Frame;
            if (l != null)
                rootframe.Navigate(typeof(DetailView), l);
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootframe = Window.Current.Content as Frame;
            rootframe.Navigate(typeof(AboutView));
        }

        private void DiscoButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)DiscoButton.IsChecked)
                api.DiscoMode(_lightsViewModel);
            else
                api.DiscoMode(false);
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {

            if ((bool)GroupButton.IsChecked)
            {
                myListView.IsItemClickEnabled = false;
                myListView.SelectionMode = ListViewSelectionMode.Multiple;
                OnOffButton.Visibility = Visibility.Visible;
                ValuesButton.Visibility = Visibility.Visible;

                Seperator.Visibility = Visibility.Collapsed;
                DiscoButton.Visibility = Visibility.Collapsed;
                RefreshButton.Visibility = Visibility.Collapsed;
                SettingsButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                myListView.IsItemClickEnabled = true;
                myListView.SelectionMode = ListViewSelectionMode.Single;
                OnOffButton.Visibility = Visibility.Collapsed;
                ValuesButton.Visibility = Visibility.Collapsed;

                Seperator.Visibility = Visibility.Visible;
                DiscoButton.Visibility = Visibility.Visible;
                RefreshButton.Visibility = Visibility.Visible;
                SettingsButton.Visibility = Visibility.Visible;
            }

        }

        private void OnOffButton_Click(object sender, RoutedEventArgs e)
        {
            bool first = false;

            if(OnOffButton.Label == "Off")
            {
                first = false;
                OnOffButton.Label = "On";
            }
            else
            {
                first = true;
                OnOffButton.Label = "Off";
            }

            if ((bool)GroupButton.IsChecked)
            {
                foreach(var v in myListView.SelectedItems)
                {
                    ((Light)v).UpdateState(first);
                }
            }
        }

        public ListView GetListView()
        {
            return myListView;
        }

        //hue.imegumii.space
        private void ValuesButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)GroupButton.IsChecked)
            {
                Frame rootframe = Window.Current.Content as Frame;
                if (myListView.SelectedItems != null)
                    rootframe.Navigate(typeof(MultiEdit), this);
            }
        }
    }
}
