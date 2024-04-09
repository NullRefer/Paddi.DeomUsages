namespace Paddi.DemoUsages.ApiDemo.Controllers;

[ApiController, Route("dicts")]
public class DictController : DemoControllerBase
{
    private readonly IDictService _service;

    public DictController(IDictService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<ApiResultDto<Dict?>>> CreateAsync([FromBody] DictDto input, CancellationToken cancellationToken = default)
        => Result(await _service.CreateAsync(input, cancellationToken));

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResultDto<Dict?>>> UpdateAsync([FromRoute] long id, [FromBody] DictDto input, CancellationToken cancellationToken = default)
        => Result(await _service.UpdateAsync(id, input, cancellationToken));

    [HttpGet("{id:long}")]
    [ResponseCache(Duration = 5)]
    public async Task<ActionResult<ApiResultDto<DictDto?>>> GetAsync([FromRoute] long id, CancellationToken cancellationToken = default)
        => Result(await _service.GetAsync(id, cancellationToken));

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResultDto<int>>> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken = default)
        => Result(await _service.DeleteAsync(id, cancellationToken));
}
