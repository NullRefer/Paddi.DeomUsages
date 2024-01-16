namespace Paddi.DemoUsages.ApiDemo.Controllers
{
    public abstract class DemoControllerBase : ControllerBase
    {
        [NonAction]
        protected ApiResultDto<T> Result<T>(AppResult<T> apiResult)
        {
            var result = new ApiResultDto<T>
            {
                Code = apiResult.Success ? 2000 : 9997,
                Msg = apiResult.Msg,
                Data = apiResult.Data
            };
            return result;
        }

        [NonAction]
        protected ApiResultDto<T> Result<T>(T value)
        {
            var result = new ApiResultDto<T>
            {
                Code = 2000,
                Msg = "Success",
                Data = value
            };
            return result;
        }
    }
}
