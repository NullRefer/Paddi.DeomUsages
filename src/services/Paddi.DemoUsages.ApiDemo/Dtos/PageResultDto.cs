namespace Paddi.DemoUsages.ApiDemo;

public abstract class PageResultDto<T>
{
    public int Code { get; set; }
    public string Msg { get; set; } = "";
    public bool Success => Code == 2000;

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public long Total { get; set; }
    public List<T> Data { get; set; } = new List<T>();
}
