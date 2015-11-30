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

        public bool IsOn { get;  set; }
        public double Brightness{ get; set; }
        public double Hue { get; set; }
        public double Saturation { get; set; }

        public APIHandler api { get; set; }
 
        public Light()
        { 
        }

        public void UpdateState(bool on)
        {
            NotifyPropertyChanged(nameof(UpdateState));
            this.IsOn = on;
            api.SetLightState(this);
            
        }

        public void UpdateColor(int r, int g, int b)
        {
            NotifyPropertyChanged(nameof(UpdateColor));
            double h; double s; double v;
            ColorUtil.RGBtoHSV(r,g,b, out h,out s,out v);
            this.Hue = h; this.Saturation = s; this.Brightness = v;
            api.SetLightValues(this);
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
