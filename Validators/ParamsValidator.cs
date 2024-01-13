using FluentValidation;
using RainfallApi.Parameters;

namespace RainfallApi.Validators;

public class ParamsValidator : AbstractValidator<Params>
{
    public ParamsValidator()
    {
        RuleFor(x => x.Count)
            .GreaterThan(0)
            .WithMessage("Count should be greater than or equal to 1");
        RuleFor(x => x.Count)
            .LessThan(101)
            .WithMessage("Count should be less than or equal to 100");
    }
}
