using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HueUWP
{
    class APIHandler
    {
        

        NetworkHandler nwh;
        public APIHandler(NetworkHandler nwh)
        {
            this.nwh = nwh;
        }

        public async void Register()
        {
            var json = await nwh.RegisterName("Hue", "Kenneth&Yorick");
            json = json.Replace("[", "").Replace("]", "");
            JObject o = JObject.Parse(json);
            string id= o["success"]["username"].ToString();
            MainPage.LOCAL_SETTINGS.Values["id"] = id;
        }

        public async void GetAllLights(ObservableCollection<Light> alllights)
        {
            List<Light> lightlist = new List<Light>();

            var json = await nwh.AllLights();
            Debug.WriteLine(json);
            JObject o = JObject.Parse(json);
            Debug.WriteLine(o.ToString());
            
            for(int  i = 1; i <= o.Count; i++)
            {
                var light = o["" + i];
                var state = light["state"];
                alllights.Add(new Light() { ID=i ,  Brightness = (int)state["bri"] , On = (bool) state["on"], Hue=(int)state["hue"], Saturation=(int)state["sat"],Name=(string)light["name"] , Type=(string)light["type"]});
                Debug.WriteLine("Added light number " + i);
            }
        }
    }
}
