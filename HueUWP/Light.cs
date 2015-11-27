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
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }

        public bool On { get; private set; }
        public int Brightness{ get; private set; }
        public int Hue { get; private set; }
        public int Saturation { get; private set; }
 
        public Light()
        {
            Debug.WriteLine("Get lamp data on creation or something");
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
            Debug.WriteLine(on);
            
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
            if (_lights.Count == 0)
            {
                _lights.Add(new Light());
            }
            return _lights;
        }
    }


   

}
