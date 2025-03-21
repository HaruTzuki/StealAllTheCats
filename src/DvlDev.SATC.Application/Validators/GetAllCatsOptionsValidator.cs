using DvlDev.SATC.Application.Models;
using FluentValidation;

namespace DvlDev.SATC.Application.Validators;

public class GetAllCatsOptionsValidator : AbstractValidator<GetAllCatsOptions>
{
	private static readonly string[] AcceptedOrderByValues = ["id", "catid", "width", "height"];

	public GetAllCatsOptionsValidator()
	{
		RuleFor(x => x.SortField)
			.Must(x => x is null || AcceptedOrderByValues.Contains(x, StringComparer.OrdinalIgnoreCase))
			.WithMessage("You can only order by: " + string.Join(", ", AcceptedOrderByValues));

		RuleFor(x => x.Page)
			.GreaterThanOrEqualTo(1);

		RuleFor(x => x.PageSize)
			.InclusiveBetween(1, 25)
			.WithMessage("Page size must be between 1 and 25.");
	}
}