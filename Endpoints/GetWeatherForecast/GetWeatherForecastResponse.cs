namespace Authentication.Endpoints.GetWeatherForecast
{
    public class GetWeatherForecastResponse
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}
