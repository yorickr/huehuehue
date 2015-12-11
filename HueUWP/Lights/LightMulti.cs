using HueUWP.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueUWP.Lights
{
    class LightMulti : Light
    {
        private List<Light> _lights; 

        public LightMulti(List<Light> lights) : base(0, "Multiple Lights", (lights.ToDelimitedString(l => l.Name)))
        {
            _lights = lights;
            Light l = _lights.First();

            SaturationEnabled = true;
            HueEnabled = true;
            BrightnessEnabled = true;

            _isOn = l.IsOn;

            _hue = l.Hue;
            _brightness = l.Brightness;
            _saturation = l.Saturation;

            NotifyPropertyChanged(nameof(IsOn));
            NotifyPropertyChanged(nameof(Hue));
            NotifyPropertyChanged(nameof(Saturation));
            NotifyPropertyChanged(nameof(Brightness));
        }

        public override void SetColor(bool instant = false)
        {
            _lights.ForEach(l => { l.SetColor(instant); });
        }

        public override void SetState()
        {
            _lights.ForEach(l => { l.SetState(); });
        }

        public override async Task<String> Update()
        {
            //Do nothing, performance reasons
            return "success";
        }

        protected override void NotifyPropertyChanged(string propertyName)
        {
            base.NotifyPropertyChanged(propertyName);
            _lights.ForEach(l => { l.IsOn = IsOn; l.Hue = Hue; l.Saturation = Saturation; l.Brightness = Brightness; });
        }
    }
}
