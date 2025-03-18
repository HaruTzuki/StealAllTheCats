using DvlDev.SATC.Application.Models;
using FluentValidation;

namespace DvlDev.SATC.Application.Validators;

public class CatValidator : AbstractValidator<Cat>
{
    public CatValidator()
    {
        RuleFor(cat => cat.CatId)
            .NotEmpty();

        RuleFor(cat => cat.Width)
            .GreaterThan(0);
        
        RuleFor(cat => cat.Height)
            .GreaterThan(0);
    }
}