using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using MudBlazor;

namespace BlazorWeatherCUSI.Data

{
    public class WeatherForecastService
    {
        private readonly HttpClient http;

        private readonly Dictionary<string, string> Images = new Dictionary<string, string> {
            {"clear sky", "https://media.gettyimages.com/videos/timelapse-blue-sky-video-id992941878?s=640x640"},
            {"few clouds", "https://bitcoinist.com/wp-content/uploads/2019/07/10th-July-8.jpg"},
            { "scattered clouds", "https://live.staticflickr.com/2106/1909487867_de140c7eb8_b.jpg"},
            { "broken clouds", "https://images.fineartamerica.com/images/artworkimages/mediumlarge/1/broken-clouds-darrell-maciver.jpg"},
            { "shower rain", "https://www.vmcdn.ca/f/files/via/images/weather/rain-umbrella-vancouver-weather.jpg;w=960"},
            { "rain", "https://www.treehugger.com/thmb/6u0wbqYvi-7wxKEoueucTLDcttM=/768x0/filters:no_upscale():max_bytes(150000):strip_icc()/__opt__aboutcom__coeus__resources__content_migration__mnn__images__2017__03__raindrops-plants-smell-57c4545c1bb844c0a2cd7e0252cf6d2f.jpg"},
            { "thunderstorm","https://horizon-media.s3-eu-west-1.amazonaws.com/s3fs-public/field/image/milan_thunderstorm_sm.jpg"},
            { "snow","https://s7d2.scene7.com/is/image/TWCNews/snowflake-formatted-snow-03222020jpg"},
            { "mist","https://cff2.earth.com/uploads/2018/11/13053559/what-is-mist.jpg"} };

        public WeatherForecastService(HttpClient http)
        {
            this.http = http;
        }

        // Request to Current weather API
        public async Task<WeatherModel> GetCurrentWeather(string ZipCode)
        {
            WeatherModel data = new();
            try {
                string endpoint = $"http://api.openweathermap.org/data/2.5/weather?zip={ZipCode},us&units=imperial&appid=8ef31ef8cb7883c2d74f94951382244f";
                string Response = await http.GetStringAsync(endpoint);

                JObject JsonData = JObject.Parse(Response);

                // Name
                data.CityName = JsonData["name"] != null ? JsonData["name"].ToString() : "Unknown City";
               

                // Coordinates
                data.Latitude = (double)JsonData["coord"]["lat"];
                data.Longitude = (double)JsonData["coord"]["lon"];

                // Sys
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                data.Sunrise = JsonData["sys"]["sunrise"] != null ? dateTime.AddSeconds((double)JsonData["sys"]["sunrise"]).ToLocalTime().ToLongTimeString() : dateTime.ToLongTimeString();
                data.Sunset = JsonData["sys"]["sunset"] != null ? dateTime.AddSeconds((double)JsonData["sys"]["sunset"]).ToLocalTime().ToLongTimeString() : dateTime.ToLongTimeString();
                data.Country = JsonData["sys"]["country"] != null ? JsonData["sys"]["country"].ToString() : "Unknown Country";

                // Main
                data.Temperature = JsonData["main"]["temp"] != null ? (double)JsonData["main"]["temp"] : 0.0;
                data.Pressure = JsonData["main"]["pressure"] != null ? (long)JsonData["main"]["pressure"] : 0;
                data.FeelsLike = JsonData["main"]["feels_like"]!=null ? (double)JsonData["main"]["feels_like"]: 0.0;
                data.Humidity = JsonData["main"]["humidity"] != null ? (int)JsonData["main"]["humidity"] : 0;
                data.MaxTemperature = JsonData["main"]["temp_max"] != null ? (double)JsonData["main"]["temp_max"] : 0.0;
                data.MinTemperature = JsonData["main"]["temp_min"] != null ? (double)JsonData["main"]["temp_min"] : 0.0;
                
                // Weather
                data.Description = JsonData["weather"][0]["description"] != null ? JsonData["weather"][0]["description"].ToString() : "Unknown";
                data.PhotoUrl = Images.ContainsKey(data.Description) == true?  Images[data.Description] : "http://sf.co.ua/14/02/wallpaper-281333.jpg";
                data.IconUrl = $"http://openweathermap.org/img/wn/{JsonData["weather"][0]["icon"]}.png";
                
                // Visibility
                data.Visibility = JsonData["visibility"] != null ? (long)JsonData["visibility"] / 1000 : 0;

                // Wind data
                data.WindSpeed = JsonData["wind"]["speed"] != null ? (double)JsonData["wind"]["speed"] : 0.0;
                data.WindGust = JsonData["wind"]["gust"] != null ? (double)JsonData["wind"]["gust"] : 0.0 ;
                data.WindDegree = JsonData["wind"]["deg"] != null ? (double)JsonData["wind"]["deg"] :0.0;
                data.WindDirection = WindDegreeToDirection(data.WindDegree);

                // Cloud data
                data.Cloudiness = JsonData["clouds"]?["all"] != null ? (double)JsonData["clouds"]["all"] :  0.0;

                // Rain data
                data.Rain1h = JsonData["rain"]?["1h"] != null ? (double)JsonData["rain"]["1h"] : 0.0;
                data.Rain3h = JsonData["rain"]?["3h"] != null ? (double)JsonData["rain"]["3h"]:0.0;

                // Snow data
                data.Snow1h = JsonData["snow"]?["1h"] != null ? (double)JsonData["snow"]["1h"]: 0.0;
                data.Snow3h = JsonData["snow"]?["3h"] != null ? (double)JsonData["snow"]["3h"]: 0.0;

                return data;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e);
                return null;
            }
          
        }

        // Convert wind degree to direction
        private static string WindDegreeToDirection(double degree)
        {
            string direction = "";
            if (degree > 348.75 || degree < 11.25)
            {
                direction = "N";
            }
            else if (degree >= 11.25 && degree < 33.75)
                direction = "NNE";
            else if (degree >= 33.75 && degree < 56.25)
                direction = "NE";
            else if (degree >= 56.25 && degree < 78.75)
                direction = "ENE";
            else if (degree >= 78.75 && degree <101.25)
                direction = "E";
            else if (degree >=101.25 && degree < 123.75)
                direction = "ESE";
            else if (degree >= 123.75 && degree <146.25)
                direction = "SE";
            else if (degree >= 146.25 && degree < 168.75)
                direction = "SSE";
            else if (degree >= 168.75 && degree < 191.25)
                direction = "S";
            else if (degree >= 191.25 && degree < 213.75)
                direction = "SSW";
            else if (degree >= 213.75 && degree < 236.25)
                direction = "SW";
            else if (degree >= 236.25 && degree < 258.75)
                direction = "WSW";
            else if (degree >= 258.75 && degree < 281.25)
                direction = "W";
            else if (degree >= 281.25 && degree < 303.75)
                direction = "WNW";
            else if (degree >= 303.75 && degree < 326.25)
                direction = "NW";
            else if (degree >= 326.25 && degree < 348.75)
                direction = "NNW";
            return direction; 
        }

        // Request to One call API
        public async Task<JObject> OneCallForecast(string lat, string lon)
        {
            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&exclude=minutely,alerts&units=imperial&appid=8ef31ef8cb7883c2d74f94951382244f";

                string OneCallApi = await http.GetStringAsync(url);
                JObject JsonData = JObject.Parse(OneCallApi);
                return JsonData;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e);
                return null;
            }
          
        }
    }
}
