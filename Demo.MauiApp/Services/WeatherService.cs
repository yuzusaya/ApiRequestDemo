using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Shared.Models;

namespace Demo.MauiApp.Services
{
    public class WeatherService : BaseApiService
    {
        public Task<List<WeatherForecast>> GetWeatherForecastList()
        {
            return GetTAsync<List<WeatherForecast>>("WeatherForecast");
        }

        public Task<HttpResponseMessage> UpdateWeatherForecast(WeatherForecast weatherForecast)
        {
            return PostTAsync("WeatherForecast", weatherForecast);
            return PutAsync("WeatherForecast", weatherForecast);
            DeleteAsync("WeatherForecast");
        }
    }
}
