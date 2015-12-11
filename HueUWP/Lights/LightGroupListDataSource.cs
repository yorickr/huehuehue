using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueUWP.Lights
{
    class LightGroupListDataSource
    {
        private ObservableCollection<Light> _groups;

        public LightGroupListDataSource()
        {
            _groups = new ObservableCollection<Light>();
        }

        public async Task<String> LoadGroups()
        {
            return await App.api.Groups(_groups);
        }

        public ObservableCollection<Light> GetGroups()
        {
            return _groups;
        }

        public void Clear()
        {
            _groups.Clear();
        }

        public async Task<String> UpdateGroups()
        {
            foreach (Light g in _groups)
            {
                await g.Update();
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }

            return "success";
        }
    }
}
