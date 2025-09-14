namespace FCG_Games.Domain.Extensions;

public static class DateExtensions
{
	public static bool BeAValidDate(this DateOnly date) => date != default && date.Year >= 1900;
}
