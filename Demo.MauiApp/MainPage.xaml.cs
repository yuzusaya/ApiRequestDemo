using Demo.MauiApp.Services;
using Demo.Shared.Models;

namespace Demo.MauiApp;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
    {
        var service = new WeatherService();
        try
        {
            var weatherForecastList = await service.GetWeatherForecastList();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}

