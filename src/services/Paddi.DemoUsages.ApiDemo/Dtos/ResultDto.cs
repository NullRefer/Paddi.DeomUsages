using System.Text.Json.Serialization;

namespace Paddi.DemoUsages.ApiDemo.Dtos;

public class ResultDto
{
    public int Code { get; set; }
    public string Msg { get; set; } = "";
    public bool Success => Code == 2000;
    public static ResultDto Fail(string msg) => new()
    {
        Code = 9997,
        Msg = msg
    };
}

public class ResultDto<T> : ResultDto
{
    public ResultDto()
    {

    }

    public ResultDto(T data)
    {
        Code = 2000;
        Msg = "Success";
        Data = data;
    }

    [JsonPropertyOrder(4)]
    [JsonProperty(Order = 4)]
    public T? Data { get; set; }

    public static new ResultDto<T> Fail(string msg) => new()
    {
        Msg = msg
    };
}
