using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HueUWP
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string IP
        {
            get { return App.LOCAL_SETTINGS.Values["ip"] as string; }
            set { App.LOCAL_SETTINGS.Values["ip"] = value; NotifyPropertyChanged(nameof(IP));  }
        }

        public string PORT
        {
            get { return Convert.ToInt32(App.LOCAL_SETTINGS.Values["port"]).ToString(); }
            set { App.LOCAL_SETTINGS.Values["port"] = int.Parse(value); NotifyPropertyChanged(nameof(PORT)); }
        }

        public bool AUTOREFRESH
        {
            get { return (bool)(App.LOCAL_SETTINGS.Values["autorefresh"]); }
            set { App.LOCAL_SETTINGS.Values["autorefresh"] = value; NotifyPropertyChanged(nameof(AUTOREFRESH)); }
        }

        public string ID
        {
            get { return App.LOCAL_SETTINGS.Values["id"] as string; }
        }

        public void Update()
        {
            NotifyPropertyChanged(nameof(ID));
        }
    }
}
