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
        private static ApplicationDataContainer LOCAL_SETTINGS = ApplicationData.Current.LocalSettings;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string IP
        {
            get { Debug.WriteLine(LOCAL_SETTINGS.Values["ip"]); return LOCAL_SETTINGS.Values["ip"] as string; }
            set { LOCAL_SETTINGS.Values["ip"] = value; NotifyPropertyChanged(nameof(IP));  }
        }

        public string PORT
        {
            get { Debug.WriteLine(LOCAL_SETTINGS.Values["port"]); return Convert.ToInt32(LOCAL_SETTINGS.Values["port"]).ToString(); }
            set { LOCAL_SETTINGS.Values["port"] = int.Parse(value); NotifyPropertyChanged(nameof(PORT)); }
        }

        public string ID
        {
            get { Debug.WriteLine(LOCAL_SETTINGS.Values["id"]); return LOCAL_SETTINGS.Values["id"] as string; }
        }

        public void Update()
        {
            NotifyPropertyChanged(nameof(ID));
        }
    }
}
