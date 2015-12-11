using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueUWP.Lights
{
    class LightGroup : Light
    {
        public LightGroup(int id, string name) : base(id, name, "Group")
        {

        }

        public override void SetColor(bool instant = false)
        {
            App.api.SetGroupColor(this, instant);
        }

        public override void SetState()
        {
            App.api.SetGroupState(this);
        }

        public override async Task<String> Update()
        {
            return await App.api.UpdateGroup(this);
        }
    }
}
