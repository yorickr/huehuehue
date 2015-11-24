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
        private async Task<String> Put(string ip, int port, string path, string json)
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

                System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }


        private async Task<String> Post(string ip, int port, string path, string json)
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

                System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        private async Task<String> Get(string ip, int port, string path)
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

                System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }


        public async Task RegisterName(string AppName, string UserName)
        {
            var response = await Post("localhost",8000,"",$"{{\"devicetype\":\"{AppName}#{UserName}\"}}");
            if (string.IsNullOrEmpty(response))
                await new MessageDialog("Error while setting username. ….").ShowAsync();
        }

        public async Task Test()
        {
            var response = await Get("localhost", 8000, "/111bb033202ac68b5812245c22f77eb/lights");
            if (string.IsNullOrEmpty(response))
                await new MessageDialog("Error while setting username. ….").ShowAsync();
        }



    }
}
