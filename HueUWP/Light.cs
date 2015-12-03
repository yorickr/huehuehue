using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Media;

namespace HueUWP
{
    public class Light : INotifyPropertyChanged
    {
        public int ID { get;  set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public bool HueEnabled;
        public bool SaturationEnabled;

        private bool _isOn;

        public bool IsOn
        {
            get { return _isOn; }
            set { _isOn = value; NotifyPropertyChanged(nameof(IsOn)); }
        }

        //Hue
        private double _hue;
        public double Hue
        {
            get { return _hue; }
            set { _hue = value; NotifyPropertyChanged(nameof(Hue)); NotifyPropertyChanged(nameof(Color)); }
        }

        //Brightness
        private double _brightness;
        public double Brightness
        {
            get { return _brightness; }
            set { _brightness = value; NotifyPropertyChanged(nameof(Brightness)); NotifyPropertyChanged(nameof(Color)); }
        }

        //Saturation
        private double _saturation;
        public double Saturation
        {
            get { return _saturation; }
            set { _saturation = value; NotifyPropertyChanged(nameof(Saturation)); NotifyPropertyChanged(nameof(Color)); }
        }

        public APIHandler api { get; set; }

        public SolidColorBrush Color
        {
            get { return new SolidColorBrush(HueUWP.ColorUtil.HsvToRgb(Hue, Saturation, Brightness)); }
        }

        public Light()
        { 
        }

        public void UpdateState(bool on)
        {
            NotifyPropertyChanged(nameof(UpdateState));
            this.IsOn = on;
            api.SetLightState(this);
            
        }

        public void UpdateColor()
        {
            api.SetLightValues(this);
        }

        public void UpdateColorDisco()
        {
            api.SetInstantLightValues(this);
        }

        public void UpdateColor(int h, int s, int b)
        {
            //double h; double s; double v;
            //ColorUtil.RGBtoHSV(r,g,b, out h,out s,out v);
            this.Hue = h; this.Saturation = s; this.Brightness = b;
            api.SetLightValues(this);
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
