using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class Program
{
    private static readonly string apiKey = "API_KEY";
    private static readonly string apiUrl = "http://api.openweathermap.org/data/2.5/weather";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter city name: ");
        string cityName = Console.ReadLine();
        
        await GetWeatherDataAsync(cityName);
    }

    public static async Task GetWeatherDataAsync(string city)
    {
        using HttpClient client = new HttpClient();
        try
        {
            // request URL
            string url = $"{apiUrl}?q={city}&appid={apiKey}&units=metric";

            // Send the GET request
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Throw if status code isn't 200

            // Read and parse the response body
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject weatherData = JObject.Parse(responseBody);

            // Extract data from the JSON
            string description = weatherData["weather"][0]["description"].ToString();
            double temp = double.Parse(weatherData["main"]["temp"].ToString());
            double feelsLike = double.Parse(weatherData["main"]["feels_like"].ToString());
            double windSpeed = double.Parse(weatherData["wind"]["speed"].ToString());

            // Output the weather data
            Console.WriteLine($"Weather in {city}:");
            Console.WriteLine($"Description: {description}");
            Console.WriteLine($"Temperature: {temp}°C (Feels like {feelsLike}°C)");
            Console.WriteLine($"Wind Speed: {windSpeed} m/s");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Request error: " + e.Message);
        }
    }
}
