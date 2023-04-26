namespace Paddi.DemoUsages.ConsoleDemo.RunnableServices.RefitRunner;

public class AmapWeatherResultDto
{
    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("count")]
    public string Count { get; set; }

    [JsonProperty("info")]
    public string Info { get; set; }

    [JsonProperty("infocode")]
    public string InfoCode { get; set; }

    [JsonProperty("lives")]
    public Live[] Lives { get; set; }
}

public class Live
{
    [JsonProperty("province")]
    public string Province { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("adcode")]
    public string Adcode { get; set; }

    [JsonProperty("weather")]
    public string Weather { get; set; }

    [JsonProperty("temperature")]
    public string Temperature { get; set; }

    [JsonProperty("winddirection")]
    public string Winddirection { get; set; }

    [JsonProperty("windpower")]
    public string Windpower { get; set; }

    [JsonProperty("humidity")]
    public string Humidity { get; set; }

    [JsonProperty("reporttime")]
    public string Reporttime { get; set; }

    [JsonProperty("temperature_float")]
    public string TemperatureFloat { get; set; }

    [JsonProperty("humidity_float")]
    public string HumidityFloat { get; set; }
}
