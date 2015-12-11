using HueUWP.Helpers;
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
using HueUWP.Lights;
using Windows.Networking.Connectivity;
using Windows.Networking;

namespace HueUWP
{
    public class APIHandler
    {
        bool discomode = false;

        public APIHandler()
        {
        }

        public async Task<String> Register()
        {
            var hostNames = NetworkInformation.GetHostNames();
            var hostName = hostNames.FirstOrDefault(name => name.Type == HostNameType.DomainName)?.DisplayName ?? "Unknown Device";

            try
            {
                var json = await NetworkHandler.RegisterName("YK Hue", hostName);
                json = json.Replace("[", "").Replace("]", "");
                JObject o = JObject.Parse(json);
                string id = o["success"]["username"].ToString();
                App.LOCAL_SETTINGS.Values["id"] = id;
                return "success";
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not register.");
                return "error";
            }
        }

        public async Task<String> UpdateGroup(Light g)
        {
            string json = await NetworkHandler.Group(g.ID);
            JObject o = JObject.Parse(json);
            var state = o["action"];

            g.IsOn = ((string)state["on"]).ToLower() == "true" ? true : false;
            if(g.HueEnabled)
                g.Hue = (int)state["hue"];
            if(g.BrightnessEnabled)
                g.Brightness = (int)state["bri"];
            if(g.SaturationEnabled)
                g.Saturation = (int)state["sat"];

            return "success";

        }

        public async Task<String> UpdateLight(Light l)
        {
            string json = await NetworkHandler.Light(l.ID);
            JObject o = JObject.Parse(json);

            Debug.WriteLine(o);

            var state = o["state"];

            l.IsOn = ((string)state["on"]).ToLower() == "true" ? true : false;
            if(l.HueEnabled)
                l.Hue = (int)state["hue"];
            if(l.BrightnessEnabled)
                l.Brightness = (int)state["bri"];
            if(l.SaturationEnabled)
                l.Saturation = (int)state["sat"];

            return "success";
        }

        public async Task<String> SetLightState(Light l)
        {
            string json = "error";
            if (l is LightSingle)
                json = await NetworkHandler.SetLight(l.ID, l.IsOn);
            return json;
        }


        public async Task<String> SetLightColor(Light l, bool instant = false)
        {
            if(l is LightSingle && l.IsOn)
            {
                var json = await NetworkHandler.SetLight(l.ID, l.Hue, l.Saturation, l.Brightness, instant);
                return "success";
            }  
            return "error";
        }

        public async Task<String> SetGroupState(Light l)
        {
            string json = "error";
            if (l is LightGroup)
                json = await NetworkHandler.SetGroup(l.ID, l.IsOn);
            return json;
        }


        public async Task<String> SetGroupColor(Light l, bool instant = false)
        {
            if (l is LightGroup && l.IsOn)
            {
                var json = await NetworkHandler.SetGroup(l.ID, l.Hue, l.Saturation, l.Brightness, instant);
                return "success";
            }

            return "error";
        }

        public async Task<String> Groups(ObservableCollection<Light> groups)
        {
            try
            {
                var json = await NetworkHandler.Groups();
                JObject o = JObject.Parse(json);
                foreach (var i in o)
                {
                    var groupjson = await NetworkHandler.Group(Int32.Parse(i.Key));
                    JObject o2 = JObject.Parse(groupjson);
                    var state = o2["action"];

                    Light g = new LightGroup(Int32.Parse(i.Key), (string)o2["name"]);

                    g.IsOn = ((string)state["on"]).ToLower() == "true" ? true : false;

                    //Hue
                    if (!state["hue"].IsNullOrEmpty())
                    {
                        g.HueEnabled = true;
                        g.Hue = (int)state["hue"];
                    }
                    else
                        g.HueEnabled = false;

                    //Brightness
                    if (!state["bri"].IsNullOrEmpty())
                    {
                        g.BrightnessEnabled = true;
                        g.Brightness = (int)state["bri"];
                    }
                    else
                        g.BrightnessEnabled = false;

                    //Saturation
                    if (!state["sat"].IsNullOrEmpty())
                    {
                        g.SaturationEnabled = true;
                        g.Saturation = (int)state["sat"];
                    }
                    else
                        g.SaturationEnabled = false;

                    groups.Add(g);

                }
                return "success";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("Could not get all groups.");
                return "error";
            }
        }

        public async Task<String> Lights(ObservableCollection<Light> lights)
        {

            //List<Light> _lights = new List<Light>();

            try {
                var json = await NetworkHandler.Lights();
                JObject o = JObject.Parse(json);
                foreach(var i in o)
                {
                    var light = o["" + i.Key];
                    var state = light["state"];

                    Light l = new LightSingle(Int32.Parse(i.Key), (string)light["name"], (string)light["type"]);

                    l.IsOn = ((string)state["on"]).ToLower() == "true" ? true : false;

                    //Hue
                    if (!state["hue"].IsNullOrEmpty())
                    {
                        l.HueEnabled = true;
                        l.Hue = (int)state["hue"];
                    }
                    else
                        l.HueEnabled = false;

                    //Brightness
                    if (!state["bri"].IsNullOrEmpty())
                    {
                        l.BrightnessEnabled = true;
                        l.Brightness = (int)state["bri"];
                    }
                    else
                        l.BrightnessEnabled = false;

                    //Saturation
                    if (!state["sat"].IsNullOrEmpty())
                    {
                        l.SaturationEnabled = true;
                        l.Saturation = (int)state["sat"];
                    }
                    else
                        l.SaturationEnabled = false;

                    lights.Add(l);
                }

                //if(_lights.Count > 0)
                //{
               //     _lights.OrderBy(l => l.Name);
                //    lights = new ObservableCollection<Light>(_lights);
                //}

                return "success";
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("Could not get all lights.");
                return "error";
            }
        }


        public async void DiscoMode(ObservableCollection<Light> lights)
        {
            discomode = true;
            Random rnd = new Random();
            while (discomode)
            {
                lights.ToList().Where(l => l.IsOn == true).ToList().ForEach(l =>
                {
                    l.Hue = rnd.Next(0, 65535);
                    l.Brightness = rnd.Next(214, 254);
                    l.Saturation = 254;
                    l.SetColor(true);
                });
                await Task.Delay(TimeSpan.FromMilliseconds(lights.Count * 100));
            }

        }

        public async void DiscoMode(bool on)
        {
            discomode = on;
        }
    }
}
