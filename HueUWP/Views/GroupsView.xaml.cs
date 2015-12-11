using HueUWP.Lights;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace HueUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupsView : Page
    {

        private LightGroupListDataSource _groupsViewModel;
        public ObservableCollection<Light> GroupsViewModel
        {
            get { return _groupsViewModel.GetGroups(); }
        }

        DispatcherTimer timer;

        public GroupsView()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            _groupsViewModel = new LightGroupListDataSource();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            QuietReloadGroups();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ReloadGroups();
            this.DataContext = GroupsViewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            timer.Stop();
        }

        private async void ReloadGroups()
        {
            timer.Stop();
            ErrorMessage.Text = "";
            FeedbackPanel.Visibility = Visibility.Visible;
            _groupsViewModel.Clear();
            

            Loading.IsActive = true;
            string s = await _groupsViewModel.LoadGroups();

            if (s == "error")
                ErrorMessage.Text = "There was a problem connecting...";
            else if (_groupsViewModel.GetGroups().Count < 1)
                ErrorMessage.Text = "No groups found...";
            else
            {
                FeedbackPanel.Visibility = Visibility.Collapsed;
                timer.Start();
            }

            Loading.IsActive = false;
        }

        private async void QuietReloadGroups()
        {
            string s = await _groupsViewModel.UpdateGroups();
        }

        private void GroupsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Light g = e.ClickedItem as Light;
            Frame rootframe = Window.Current.Content as Frame;
            if (g != null)
                rootframe.Navigate(typeof(DetailView), g);
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch button = ((ToggleSwitch)sender);
            Light group = (Light)button.DataContext;
            if (group != null)
                group.SetState();
        }
    }
}
