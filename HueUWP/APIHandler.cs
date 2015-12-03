using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Windows.Storage;

namespace HueUWP
{
    public class APIHandler
    {
        bool discomode = false;

        NetworkHandler nwh;
        public APIHandler(NetworkHandler nwh)
        {
            this.nwh = nwh;
        }

        public async Task<String> Register()
        {
            try
            {
                var json = await nwh.RegisterName("YK Hue", "YK");
                json = json.Replace("[", "").Replace("]", "");
                JObject o = JObject.Parse(json);
                string id = o["success"]["username"].ToString();
                MainPage.LOCAL_SETTINGS.Values["id"] = id;
                return "success";
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not register.");
                return "error";
            }
        }

        public async void SetLightState(Light l)
        {
            var json = await nwh.SetLightInfo(l.ID, $"{{\"on\": {((l.IsOn) ? "true" : "false")}}}");
            Debug.WriteLine(json);
        }

        public async void SetLightState(List<Light> lights)
        {
            lights.ForEach(l => SetLightState(l));
        }

        public async void DiscoMode(ObservableCollection<Light> lights)
        {
            discomode = true;
            Random rnd = new Random();
            while(discomode)
            {
                lights.ToList().ForEach(l =>
                {
                    l.Hue = rnd.Next(0, 65535);
                    l.Brightness = rnd.Next(128, 254);
                    l.UpdateColorDisco();
                });
                //"transitiontime": 0
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
            
        }

        public async void DiscoMode(bool on)
        {
            discomode = on;
        }

        public async Task<String> SetLightValues(Light l)
        {
            if(l.IsOn)
            {
                Debug.WriteLine(l.Hue);
                var json = await nwh.SetLightInfo(l.ID, $"{{\"bri\": {l.Brightness},\"hue\": {(l.Hue)},\"sat\": {l.Saturation}}}");
                Debug.WriteLine(json);
                return "success";
            }
            
            return "error";
        }
        public async Task<String> SetInstantLightValues(Light l)
        {
            if (l.IsOn)
            {
                Debug.WriteLine(l.Hue);
                var json = await nwh.SetLightInfo(l.ID, $"{{\"bri\": {l.Brightness},\"hue\": {(l.Hue)},\"sat\": {l.Saturation}, \"transitiontime\" : 0}}");
                Debug.WriteLine(json);
                return "success";
            }

            return "error";
        }

        public async void SetLightValues(List<Light> lights)
        {
            lights.ForEach(l => SetLightValues(l));
        }

        public async Task<String> GetAllLights(ObservableCollection<Light> alllights)
        {
           // List<Light> lightlist = new List<Light>();
            try {
                var json = await nwh.AllLights();
                JObject o = JObject.Parse(json);
                Debug.WriteLine(o.ToString()); 
                foreach(var i in o)
                {
                    var light = o["" + i.Key];
                    var state = light["state"];
                    if ((String)light["type"] != "Dimmable light")
                    {
                        alllights.Add(new Light() { api = this, ID = Int32.Parse(i.Key), Brightness = (int)state["bri"], SaturationEnabled = true, HueEnabled = true, IsOn = ((string)state["on"]).ToLower() == "true" ? true : false, Hue = (int)state["hue"], Saturation = (int)state["sat"], Name = (string)light["name"], Type = (string)light["type"] });
                    }
                    else
                    {
                        alllights.Add(new Light() { api = this, ID = Int32.Parse(i.Key), Brightness = (int)state["bri"], SaturationEnabled = false,HueEnabled = false,IsOn = ((string)state["on"]).ToLower() == "true" ? true : false, Hue =0, Saturation = 0, Name = (string)light["name"], Type = (string)light["type"] });
                    }//Debug.WriteLine("Added light number " + i + " " + state["on"]);
                }
                return "success";
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("Could not get all lights.");
                return "error";
            }
        }


    }
}
