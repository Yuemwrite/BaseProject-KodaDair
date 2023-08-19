using FluentValidation;
using FluentValidation.Results;

namespace BaseProject.Application.Common.Validation;

public class CustomValidator<T> : AbstractValidator<T>
{
    public override ValidationResult Validate(ValidationContext<T> context)
    {
        return base.Validate(context);
    }
}