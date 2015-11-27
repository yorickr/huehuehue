using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        private JObject ObjectifyfJson(string json)
        {
            json = json.Replace("[", "").Replace("]", "");
            return JObject.Parse(json);
        }

        public async void Register()
        {
            var json = await nwh.RegisterName("Hue", "Kenneth&Yorick");
            JObject o = ObjectifyfJson(json);
            string id= o["success"]["username"].ToString();
            MainPage.LOCAL_SETTINGS.Values["id"] = id;
        }
    }
}
