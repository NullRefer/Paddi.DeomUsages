using Paddi.DemoUsages.ApiDemo.Dtos.Dict;

namespace Paddi.DemoUsages.ApiDemo.Controllers;

[ApiController, Route("dicts")]
public class DictController : ControllerBase
{
    private readonly IDictService _service;

    public DictController(IDictService service) => _service = service;

    [HttpPost()]
    public async Task<ActionResult<ResultDto<Dict>>> CreateAsync(DictDto input)
    {
        var result = await _service.CreateAsync(input);
        if (result != null) return this.Result(result);

        return ResultDto<Dict>.Fail("Fails to create dict");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ResultDto<long>>> DeleteAsync([FromRoute] long id)
    {
        var result = await _service.DeleteAsync(id);
        return this.Result(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ResultDto<Dict?>>> UpdateAsync([FromRoute] long id, [FromBody] DictDto input)
    {
        var result = await _service.UpdateAsync(id, input);

        return this.Result(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ResultDto<Dict?>>> GetAsync([FromRoute] long id)
    {
        return this.Result(await _service.GetAsync(id));
    }
}
