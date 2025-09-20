using FCG_Games.Domain.DTO;
using FCG_Games.Domain.Extensions;
using FluentValidation;

namespace FCG_Games.Application.Validators.Game;

public class CreateGameValidator : AbstractValidator<CreateGameDto>
{
	public CreateGameValidator()
	{
		RuleFor(dto => dto.Title)
			.NotEmpty().WithMessage("Title is required.");
		RuleFor(dto => dto.Price)
			.NotEmpty().WithMessage("Price is required.")
			.GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");
		RuleFor(dto => dto.ReleaseDate)
			.NotEmpty().WithMessage("ReleaseDate is required")
			.Must(date => date.BeAValidDate()).WithMessage("The Release Date must be a valid date");
	}
}
