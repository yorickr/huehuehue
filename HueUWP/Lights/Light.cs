using HueUWP.Helpers;
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

namespace HueUWP.Lights
{
    public abstract class Light : INotifyPropertyChanged
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }

        public bool HueEnabled {get; set; }
        public bool SaturationEnabled { get; set; }
        public bool BrightnessEnabled { get; set; }

        public Light(int id, string name, string type)
        {
            this.ID = id;
            this.Name = name;
            this.Type = type;
        }

        //IsOn
        protected bool _isOn = false;
        public bool IsOn
        {
            get { return _isOn; }
            set { _isOn = value; NotifyPropertyChanged(nameof(IsOn)); }
        }

        //Hue
        protected int _hue = 0;
        public int Hue
        {
            get { return _hue; }
            set { _hue = value; NotifyPropertyChanged(nameof(Hue)); NotifyPropertyChanged(nameof(Color)); }
        }

        //Brightness
        protected int _brightness = 0;
        public int Brightness
        {
            get { return _brightness; }
            set { _brightness = value; NotifyPropertyChanged(nameof(Brightness)); NotifyPropertyChanged(nameof(Color)); }
        }

        //Saturation
        protected int _saturation = 0;
        public int Saturation
        {
            get { return _saturation; }
            set { _saturation = value; NotifyPropertyChanged(nameof(Saturation)); NotifyPropertyChanged(nameof(Color)); }
        }

        public SolidColorBrush Color
        {
            get { return new SolidColorBrush(ColorUtil.HsvToRgb(Hue, Saturation, Brightness)); }
        }

        public abstract void SetState();

        public abstract void SetColor(bool instant = false);

        public abstract Task<String> Update();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
 

}
