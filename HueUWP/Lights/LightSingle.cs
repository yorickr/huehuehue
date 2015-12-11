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
    public class LightSingle : Light
    {
        public LightSingle(int id, string name, string type) : base (id, name, type)
        {
        }

        public override void SetState()
        {
            App.api.SetLightState(this);
        }

        public override void SetColor(bool instant = false)
        {
            App.api.SetLightColor(this, instant);
        }

        public override async Task<String> Update()
        {
            return await App.api.UpdateLight(this);
        }
    }


}
