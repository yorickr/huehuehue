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
    public class APIHandler
    {
        

        NetworkHandler nwh;
        public APIHandler(NetworkHandler nwh)
        {
            this.nwh = nwh;
        }

        public async void Register()
        {
            try {
                var json = await nwh.RegisterName("Hue", "Kenneth&Yorick");
                json = json.Replace("[", "").Replace("]", "");
                JObject o = JObject.Parse(json);
                string id = o["success"]["username"].ToString();
                MainPage.LOCAL_SETTINGS.Values["id"] = id; }
            catch(Exception e)
            {
                Debug.WriteLine("Could not register.");
            }
        }

        public async void SetLightState(Light l)
        {
            var json = await nwh.SetLightInfo(l.ID, $"{{\"on\": {((l.IsOn) ? "true" : "false")}}}");
            Debug.WriteLine(json);
        }

        public async void SetLightValues(Light l)
        {
            if(l.IsOn)
            {
                Debug.WriteLine(l.Hue);
                var json = await nwh.SetLightInfo(l.ID, $"{{\"bri\": {l.Brightness},\"hue\": {(l.Hue)},\"sat\": {l.Saturation}}}");
                Debug.WriteLine(json);
            }
            
        }

        public async void GetAllLights(ObservableCollection<Light> alllights)
        {
           // List<Light> lightlist = new List<Light>();
            try {
                var json = await nwh.AllLights();
                JObject o = JObject.Parse(json);
                foreach(var i in o)
                {
                    var light = o["" + i.Key];
                    var state = light["state"];
                    alllights.Add(new Light() { api = this,ID = Int32.Parse( i.Key), Brightness = (int)state["bri"], IsOn = ((string)state["on"]).ToLower() == "true" ? true : false, Hue = (int)state["hue"], Saturation = (int)state["sat"], Name = (string)light["name"], Type = (string)light["type"] });
                    //Debug.WriteLine("Added light number " + i + " " + state["on"]);
                }

                //lightlist = lightlist.OrderBy(q => q.Name).ToList();

                //lightlist.Sort(
                //    delegate (Light p1, Light p2)
                //   {
                //       return p1.Name.CompareTo(p2.Name);
                //   }
                // );

                //lightlist.ForEach(q => alllights.Add(q));
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("Could not get all lights.");
            }
        }
    }
}
