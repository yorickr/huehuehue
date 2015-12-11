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
            get { Debug.WriteLine(App.LOCAL_SETTINGS.Values["ip"]); return App.LOCAL_SETTINGS.Values["ip"] as string; }
            set { App.LOCAL_SETTINGS.Values["ip"] = value; NotifyPropertyChanged(nameof(IP));  }
        }

        public string PORT
        {
            get { Debug.WriteLine(App.LOCAL_SETTINGS.Values["port"]); return Convert.ToInt32(App.LOCAL_SETTINGS.Values["port"]).ToString(); }
            set { App.LOCAL_SETTINGS.Values["port"] = int.Parse(value); NotifyPropertyChanged(nameof(PORT)); }
        }

        public string ID
        {
            get { Debug.WriteLine(App.LOCAL_SETTINGS.Values["id"]); return App.LOCAL_SETTINGS.Values["id"] as string; }
        }

        public void Update()
        {
            NotifyPropertyChanged(nameof(ID));
        }
    }
}
