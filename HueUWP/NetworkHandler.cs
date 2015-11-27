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
    class NetworkHandler
    {
        string ip;
        int port;
        public NetworkHandler()
        {
            this.ip = (string) MainPage.LOCAL_SETTINGS.Values["ip"];
            this.port = (int)MainPage.LOCAL_SETTINGS.Values["port"];
        }

        private async Task<String> Put(string path, string json)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();
                HttpStringContent content = new HttpStringContent(json, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application /json");

                Uri uriLampState = new Uri($"http://{ip}:{port}/api" + path);
                var response = await client.PostAsync(uriLampState, content).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                //System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
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

                Uri uriLampState = new Uri($"http://{ip}:{port}/api" + path);
                var response = await client.PostAsync(uriLampState, content).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                //System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
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

                Uri uriLampState = new Uri($"http://{ip}:{port}/api" + path);
                var response = await client.GetAsync(uriLampState).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                //System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }


        public async Task<String> RegisterName(string AppName, string UserName)
        {
            var response = await Post("",$"{{\"devicetype\":\"{AppName}#{UserName}\"}}");
            if (string.IsNullOrEmpty(response))
                await new MessageDialog("Error while setting username. ….").ShowAsync();
            return response;
        }

        public async Task<String> Test()
        {
            var response = await Get( "/111bb033202ac68b5812245c22f77eb/lights");
            if (string.IsNullOrEmpty(response))
                await new MessageDialog("Error while setting username. ….").ShowAsync();
            return response;
        }



    }
}
