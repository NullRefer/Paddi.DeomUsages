using FluentValidation;

namespace Paddi.DemoUsages.ApiDemo.Dtos;

public class DictDto
{
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
}

public class DictDtoValidator : AbstractValidator<DictDto>
{
    public DictDtoValidator()
    {
        RuleFor(e => e.Key).NotEmpty().WithMessage("键 不能为空");
        RuleFor(e => e.Value).NotEmpty().WithMessage("值 不能为空");
    }
}
