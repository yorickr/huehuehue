using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Web.Http;

namespace HueUWP
{
    public class NetworkHandler
    {
        public NetworkHandler()
        {

        }

        private static async Task<String> Put(string path, string json)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            //System.Diagnostics.Debug.WriteLine("PUT:\n" + path +"\n"+json);

            try
            {
                HttpClient client = new HttpClient();
                HttpStringContent content = new HttpStringContent(json, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application /json");

                Uri uriLampState = new Uri($"http://{(string) App.LOCAL_SETTINGS.Values["ip"]}:{(int)App.LOCAL_SETTINGS.Values["port"]}/api/" + path);
                var response = await client.PutAsync(uriLampState, content).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return "error";
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                //System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return "error";
            }
        }


        private static async Task<String> Post(string path, string json)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();
                HttpStringContent content = new HttpStringContent(json, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application /json");

                Uri uriLampState = new Uri($"http://{(string) App.LOCAL_SETTINGS.Values["ip"]}:{(int)App.LOCAL_SETTINGS.Values["port"]}/api/" + path);
                var response = await client.PostAsync(uriLampState, content).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return "error";
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                //System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return "error";
            }
        }

        private static async Task<String> Get(string path)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();
                //HttpStringContent content = new HttpStringContent($"{{\"devicetype\":\"Test#Test\"}}", Windows.Storage.Streams.UnicodeEncoding.Utf8, "application /json");

                Uri uriLampState = new Uri($"http://{(string) App.LOCAL_SETTINGS.Values["ip"]}:{(int)App.LOCAL_SETTINGS.Values["port"]}/api/" + path);
                var response = await client.GetAsync(uriLampState).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return "error";
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                //System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return "error";
            }
        }

        public static async Task<String> Lights()
        {
            var response = await Get($"{(String)App.LOCAL_SETTINGS.Values["id"]}/lights");
            if (string.IsNullOrEmpty(response))
                return "error";
            return response;
        }

        public static async Task<String> Light(int id)
        {
            var response = await Get($"{(String)App.LOCAL_SETTINGS.Values["id"]}/lights/{id}");
            if (string.IsNullOrEmpty(response))
                return "error";
            return response;
        }

        public static async Task<String> SetLight(int lightid, string json)
        {
            var response = await Put($"{(String)App.LOCAL_SETTINGS.Values["id"]}/lights/{lightid}/state", json);
            return response;
        }

        public static async Task<String> SetLight(int lightid, bool state)
        {
            string json = $"{{\"on\": { ((state) ? "true" : "false") }}}";
            var response = await Put($"{(String)App.LOCAL_SETTINGS.Values["id"]}/lights/{lightid}/state", json);
            return response;
        }

        public static async Task<String> SetLight(int lightid, int hue, int saturation, int brightness, bool instant = false)
        {
            string json = $"{{\"hue\": {(hue)},\"bri\": {brightness},\"sat\": {saturation} {(instant ? ",\"transitiontime\":0" : "")}}}";
            var response = await Put($"{(String)App.LOCAL_SETTINGS.Values["id"]}/lights/{lightid}/state", json);
            return response;
        }




        public static async Task<String> RegisterName(string AppName, string UserName)
        {
            var response = await Post("",$"{{\"devicetype\":\"{AppName}#{UserName}\"}}");
            if (string.IsNullOrEmpty(response))
                return "error";
            return response;
        }




        public static async Task<String> Groups()
        {
            var response = await Get($"{(String)App.LOCAL_SETTINGS.Values["id"]}/groups");
            if (string.IsNullOrEmpty(response))
                return "error";
            return response;
        }

        public static async Task<String> Group(int id)
        {
            var response = await Get($"{(String)App.LOCAL_SETTINGS.Values["id"]}/groups/{id}");
            if (string.IsNullOrEmpty(response))
                return "error";
            return response;
        }

        public static async Task<String> SetGroup(int groupid, string json)
        {
            var response = await Put($"{(String)App.LOCAL_SETTINGS.Values["id"]}/groups/{groupid}/action", json);
            return response;
        }

        public static async Task<String> SetGroup(int groupid, bool state)
        {
            string json = $"{{\"on\": { ((state) ? "true" : "false") }}}";
            var response = await Put($"{(String)App.LOCAL_SETTINGS.Values["id"]}/groups/{groupid}/action", json);
            return response;
        }

        public static async Task<String> SetGroup(int groupid, int hue, int saturation, int brightness, bool instant = false)
        {
            string json = $"{{\"hue\": {(hue)},\"bri\": {brightness},\"sat\": {saturation} {(instant ? ",\"transitiontime\":0" : "")}}}";
            var response = await Put($"{(String)App.LOCAL_SETTINGS.Values["id"]}/groups/{groupid}/action", json);
            return response;
        }

    }
}
