using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HueUWP
{
    public class Light : INotifyPropertyChanged
    {
        public int ID { get;  set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public bool On { get;  set; }
        public int Brightness{ get; set; }
        public int Hue { get; set; }
        public int Saturation { get; set; }

        public APIHandler api { get; set; }
 
        public Light()
        {
            //temp
            ID = 1;
            Name = "Lamp 1";
            Type = "Extended color light";
            On = true;
            Brightness = 10;
            Hue = 14215;
            Saturation = 425;
        }

        public void UpdateState(bool on)
        {
            NotifyPropertyChanged(nameof(UpdateState));
            this.On = on;
            api.SetLightData(this);
            
        }

        public void UpdateColor(int r, int g, int b)
        {
            NotifyPropertyChanged(nameof(UpdateColor));
            Debug.WriteLine(r + "-" + g + "-" + b);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class LightDataSource
    {
        private static ObservableCollection<Light> _lights
            = new ObservableCollection<Light>();

        public static ObservableCollection<Light> GetLights()
        {
            return _lights;
        }
    }


   

}
