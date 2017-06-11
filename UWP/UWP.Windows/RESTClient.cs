using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UWP
{
    class RESTClient
    {
        private string APIBaseAddress = "http://api.openweathermap.org/data/2.5/";
        private string weather = "weather?q=";
        private string apiKey = "52ed52304ece1cfeaee3e237bf6c2094";
        private string units = "&units=metric";
        public string getWeatherForLocation(string location)
        {
            var uri = new Uri(APIBaseAddress + weather + location + units);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            HttpResponseMessage response;
            response = client.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            return content;
        }
    }
}
