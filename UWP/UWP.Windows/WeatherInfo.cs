using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP
{
    class WeatherInfo
    {
        [JsonProperty("name")]
        public string city { get; set; }

        [JsonProperty("main")]
        WeatherInfoInside weatherInfoInside { get; set; }

        [JsonProperty("weather")]
        List<Weather> weather { get; set; }

        public WeatherInfo() { }

        public string getHumidityAsString()
        {
            return String.Format("{0}%", weatherInfoInside.humidity).ToString();
        }

        public string getTemperatureAsString()
        {
            return String.Format("{0} °C", weatherInfoInside.temperature);
        }

        public string getWeatherDescription()
        {
            return weather[0].description;
        }
    }

    class WeatherInfoInside
    {
        [JsonProperty("temp")]
        public long temperature { get; set; }

        [JsonProperty]
        public int humidity { get; set; }
    }

    class Weather
    {
        [JsonProperty("description")]
        public string description { get; set; }
        Weather()
        {

        }
    }
}
