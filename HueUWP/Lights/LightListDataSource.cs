using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueUWP.Lights
{

    public class LightListDataSource
    {
        private ObservableCollection<Light> _lights;

        public LightListDataSource()
        {
            _lights = new ObservableCollection<Light>();
        }

        public async Task<String> LoadLights()
        {
            return await App.api.Lights(_lights);
        }

        public ObservableCollection<Light> GetLights()
        {
            return _lights;
        }

        public void Clear()
        {
            _lights.Clear();
        }

        public async Task<String> UpdateLights()
        {
            foreach(Light l in _lights)
            {
                await l.Update();
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }

            return "success";
        }
    }
}
