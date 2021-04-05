using System;

namespace BlazorWeatherCUSI.Data
{
    public class WeatherModel
    {
        public string CityName { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Country { get; set; }
        public string Sunset { get; set; }
        public string Sunrise { get; set; }

        public double Cloudiness { get; set; }

        public double Rain1h { get; set; }
        public double Rain3h { get; set; }

        public double Snow1h { get; set; }
        public double Snow3h { get; set; }


        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string PhotoUrl { get; set; }

        public long Visibility { get; set; }

        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public long Pressure { get; set; }
        public int Humidity { get; set; }

        public double WindSpeed { get; set; }
        public double WindDegree { get; set; }
        public string WindDirection { get; set; }
        public double WindGust { get; set; }
    }
}
