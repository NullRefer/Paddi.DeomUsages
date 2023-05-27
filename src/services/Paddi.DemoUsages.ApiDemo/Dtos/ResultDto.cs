using System.Text.Json.Serialization;

namespace Paddi.DemoUsages.ApiDemo.Dtos;

public class ResultDto
{
    public int Code { get; set; }
    public string Msg { get; set; } = "";
    public bool Success => Code == 2000;
}

public class ResultDto<T> : ResultDto
{
    public ResultDto(T data)
    {
        Code = 2000;
        Msg = "Success";
        Data = data;
    }

    [JsonPropertyOrder(4)]
    [JsonProperty(Order = 4)]
    public T Data { get; set; }
}
