using System;
using System.Collections.Generic;
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
    public sealed partial class SettingsView : Page
    {
        public SettingsViewModel SettingsViewModel = new SettingsViewModel();

        private APIHandler api;

        public SettingsView()
        {    
            this.InitializeComponent();
            //SettingsViewModel = new SettingsViewModel();
        }

        private void UpdateID_Click(object sender, RoutedEventArgs e)
        {
           api.Register();
           SettingsViewModel.Update();   
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            api = e.Parameter as APIHandler;
        }
    }
}
