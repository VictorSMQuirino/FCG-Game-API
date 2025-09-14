using FluentValidation.Results;

namespace FCG_Games.Domain.Extensions;

public static class ValidationExtensions
{
	public static IDictionary<string, string[]> ToErrorsPropertyDictionary(this List<ValidationFailure> errors)
		=> errors
		.GroupBy(e => e.PropertyName)
		.ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
}
