using FCG_Games.Domain.Enums;

namespace FCG_Games.Domain.Exceptions;

public class ValidationErrorsException(IDictionary<string, string[]> errors)
	: AppException("One or more validation errors occurred.", ErrorCode.ValidationError)
{
	public IDictionary<string, string[]> Errors { get; set; } = errors;
}
