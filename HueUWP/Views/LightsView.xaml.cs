using HueUWP.Lights;
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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HueUWP.Views
{
    public sealed partial class LightsView : Page
    {
        private LightListDataSource _lightsViewModel;
        public ObservableCollection<Light> LightsViewModel
        {
            get { return _lightsViewModel.GetLights(); }
        }

        DispatcherTimer timer;

        public LightsView()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            Application.Current.Suspending += Current_Suspending;
            Application.Current.Resuming += Current_Resuming;

            _lightsViewModel = new LightListDataSource();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(8);
            timer.Tick += Timer_Tick;
        }

        private void Current_Resuming(object sender, object e)
        {
            if ((bool)DiscoButton.IsChecked)
                App.api.DiscoMode(_lightsViewModel.GetLights());
        }

        private void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            App.api.DiscoMode(false);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if((bool)GroupButton.IsChecked)
            {
                GroupButton.IsChecked = false;
                GroupButton_Click(this, new RoutedEventArgs());
                e.Handled = true;
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            QuietReloadLights();
        }

        private void StartTimer()
        {
            if ((bool)App.LOCAL_SETTINGS.Values["autorefresh"])
            {
                QuietReloadLights();
                timer.Start();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
                ReloadLights();
            else
            {
                LightsList.SelectedItems.Clear();
                StartTimer();
            }
        
            this.DataContext = LightsViewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            timer.Stop();
        }

        private async void QuietReloadLights()
        {
            string s = await _lightsViewModel.UpdateLights();
        }

        private async void ReloadLights()
        {
            timer.Stop();
            ErrorMessage.Text = "";
            FeedbackPanel.Visibility = Visibility.Visible;
            _lightsViewModel.Clear();

            Loading.IsActive = true;
            string s = await _lightsViewModel.LoadLights();

            if (s == "error")
                ErrorMessage.Text = "There was a problem connecting...";
            else if (_lightsViewModel.GetLights().Count < 1)
                ErrorMessage.Text = "No lights found...";
            else
            {
                FeedbackPanel.Visibility = Visibility.Collapsed;
                StartTimer();
            }

            Loading.IsActive = false;
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch button = ((ToggleSwitch)sender);
            Light light = (Light)button.DataContext;
            if(light != null)
                light.SetState();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootframe = Window.Current.Content as Frame;
            rootframe.Navigate(typeof(SettingsView));
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadLights();
        }

        private void LightsList_ItemClick(object sender, ItemClickEventArgs e)
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

        private void GroupsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootframe = Window.Current.Content as Frame;
            rootframe.Navigate(typeof(GroupsView));
        }

        private void DiscoButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)DiscoButton.IsChecked)
                App.api.DiscoMode(_lightsViewModel.GetLights());
            else
                App.api.DiscoMode(false);
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {

            if ((bool)GroupButton.IsChecked)
            {
                LightsList.IsItemClickEnabled = false;
                LightsList.SelectionMode = ListViewSelectionMode.Multiple;
                OnOffButton.Visibility = Visibility.Visible;
                ValuesButton.Visibility = Visibility.Visible;

                Seperator.Visibility = Visibility.Collapsed;
                DiscoButton.Visibility = Visibility.Collapsed;
                RefreshButton.Visibility = Visibility.Collapsed;
                SettingsButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                LightsList.IsItemClickEnabled = true;
                LightsList.SelectionMode = ListViewSelectionMode.Single;
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
            bool toggle = false;

            if(OnOffButton.Label == "Off")
            {
                toggle = false;
                OnOffButton.Label = "On";
            }
            else
            {
                toggle = true;
                OnOffButton.Label = "Off";
            }

            if ((bool)GroupButton.IsChecked)
            {
                foreach(var v in LightsList.SelectedItems)
                {
                    ((Light)v).IsOn = toggle;
                    ((Light)v).SetState();
                }
            }
        }

        private void ValuesButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)GroupButton.IsChecked)
            {
                Frame rootframe = Window.Current.Content as Frame;
                if (LightsList.SelectedItems != null)
                {
                    Light multilight = new LightMulti( LightsList.SelectedItems.Cast<Light>().ToList() );
                    rootframe.Navigate(typeof(DetailView), multilight);
                }
                    
            }
        }
    }
}
