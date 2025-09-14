using FCG_Games.Domain.DTO.Promotion;
using FCG_Games.Domain.Extensions;
using FluentValidation;

namespace FCG_Games.Application.Validators.Promotion;

public class CreatePromotionValidator : AbstractValidator<CreatePromotionDto>
{
	public CreatePromotionValidator()
	{
		RuleFor(promotion => promotion.DiscountPercentage)
			.NotNull().WithMessage("The discount percentage is required.")
			.GreaterThan(0).WithMessage("The discount percentage must be greater than 0.")
			.LessThanOrEqualTo(100).WithMessage("The discount percentage must be less than or equal to 100.");
		RuleFor(promotion => promotion.Deadline)
			.NotEmpty().WithMessage("The deadline is required.")
			.Must(date => date.BeAValidDate()).WithMessage("The deadline must be a valid date.");
	}
}
