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
        //string (string) MainPage.LOCAL_SETTINGS.Values["ip"];
        //int (int)MainPage.LOCAL_SETTINGS.Values["port"];
        public NetworkHandler()
        {
            //this.(string) MainPage.LOCAL_SETTINGS.Values["ip"] = (string) MainPage.LOCAL_SETTINGS.Values["(string) MainPage.LOCAL_SETTINGS.Values["ip"]"];
            //this.(int)MainPage.LOCAL_SETTINGS.Values["port"] = (int)MainPage.LOCAL_SETTINGS.Values["(int)MainPage.LOCAL_SETTINGS.Values["port"]"];
        }

        private async Task<String> Put(string path, string json)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            //System.Diagnostics.Debug.WriteLine("PUT:\n" + path +"\n"+json);

            try
            {
                HttpClient client = new HttpClient();
                HttpStringContent content = new HttpStringContent(json, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application /json");

                Uri uriLampState = new Uri($"http://{(string) MainPage.LOCAL_SETTINGS.Values["ip"]}:{(int)MainPage.LOCAL_SETTINGS.Values["port"]}/api/" + path);
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


        private async Task<String> Post(string path, string json)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();
                HttpStringContent content = new HttpStringContent(json, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application /json");

                Uri uriLampState = new Uri($"http://{(string) MainPage.LOCAL_SETTINGS.Values["ip"]}:{(int)MainPage.LOCAL_SETTINGS.Values["port"]}/api/" + path);
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

        private async Task<String> Get(string path)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();
                //HttpStringContent content = new HttpStringContent($"{{\"devicetype\":\"Test#Test\"}}", Windows.Storage.Streams.UnicodeEncoding.Utf8, "application /json");

                Uri uriLampState = new Uri($"http://{(string) MainPage.LOCAL_SETTINGS.Values["ip"]}:{(int)MainPage.LOCAL_SETTINGS.Values["port"]}/api/" + path);
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

        public async Task<String> SetLightInfo(int lightid, string json)
        {
            var response = await Put($"{(String)MainPage.LOCAL_SETTINGS.Values["id"]}/lights/{lightid}/state", json);
            return response;
        }


        public async Task<String> RegisterName(string AppName, string UserName)
        {
            var response = await Post("",$"{{\"devicetype\":\"{AppName}#{UserName}\"}}");
            if (string.IsNullOrEmpty(response))
                return "error";
            return response;
        }

        public async Task<String> AllLights()
        {
            var response = await Get($"{(String)MainPage.LOCAL_SETTINGS.Values["id"]}/lights");
            if (string.IsNullOrEmpty(response))
                return "error";
            return response;
        }

    }
}
