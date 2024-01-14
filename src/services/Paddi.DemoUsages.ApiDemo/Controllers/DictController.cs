using Paddi.DemoUsages.ApiDemo.Dtos.Dict;

namespace Paddi.DemoUsages.ApiDemo.Controllers;

[ApiController, Route("dicts")]
public class DictController : ControllerBase
{
    private readonly IDictService _service;

    public DictController(IDictService service) => _service = service;

    [HttpPost()]
    public async Task<ActionResult<ResultDto<Dict>>> CreateAsync([FromBody] DictDto input, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(input, cancellationToken);
        if (result != null) return this.Result(result);

        return ResultDto<Dict>.Fail("Fails to create dict");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ResultDto<long>>> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        return this.Result(result);
    }

    [HttpDelete("batch")]
    public async Task<ActionResult<ResultDto<long>>> BatchDeleteAsync([FromBody] List<long> idList, CancellationToken cancellationToken = default)
    {
        var result = await _service.BatchDeleteAsync(idList, cancellationToken);
        return this.Result(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ResultDto<Dict?>>> UpdateAsync([FromRoute] long id, [FromBody] DictDto input, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, input, cancellationToken);

        return this.Result(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ResultDto<Dict?>>> GetAsync([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        return this.Result(await _service.GetAsync(id, cancellationToken));
    }
}
