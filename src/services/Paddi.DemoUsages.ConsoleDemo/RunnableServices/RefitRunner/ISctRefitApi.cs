using Refit;

namespace Paddi.DemoUsages.ConsoleDemo.RunnableServices.RefitRunner;

public interface ISctRefitApi
{
    [Get("/SCT207354TwLmMz6Nw3mDMbACPg1pzQMcg.send")]
    Task<SctResponseDto> NotifyAsync(string title, string desp);
}

public record SctResponseDto(int ErrCode, string ErrMsg, long MsgId);
