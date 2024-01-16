using System.Text.Json.Serialization;

namespace Paddi.DemoUsages.ApiDemo.Dtos;

public class AppResult<T>
{
    public string Msg { get; init; }
    public bool Success { get; init; }
    public T? Data { get; init; }

    protected AppResult(string msg, T? data)
    {
        Msg = msg;
        Data = data;
        Success = data is not null;
    }

    public static AppResult<T> Ok(T data, string msg = "Success") => new(msg, data);
    public static AppResult<T> Ng(string msg = "Failed") => new(msg, default);
}

public static class AppResult
{
    public static AppResult<T> Ok<T>(T data, string msg = "Success") => AppResult<T>.Ok(data, msg);
    public static AppResult<string> Ng(string msg = "Failed") => AppResult<string>.Ng(msg);
}

public class ApiResultDto
{
    public int Code { get; set; }
    public string Msg { get; set; } = "";
    public bool Success => Code == 2000;
    public static ApiResultDto Fail(string msg) => new()
    {
        Code = 9997,
        Msg = msg
    };
}

public class ApiResultDto<T> : ApiResultDto
{
    public ApiResultDto()
    {

    }

    public ApiResultDto(T data)
    {
        Code = 2000;
        Msg = "Success";
        Data = data;
    }

    [JsonPropertyOrder(4)]
    [JsonProperty(Order = 4)]
    public T? Data { get; set; }

    public static new ApiResultDto<T> Fail(string msg) => new()
    {
        Msg = msg
    };
}
