using System.Net;

namespace FCG_Games.Domain.Exceptions;

public class DuplicatedEntityException(string entityName, string propertyName, object attemptedValue)
	: DomainException(
		$"Already exists a {entityName} with {propertyName} equals to '{attemptedValue}.'",
		entityName,
		propertyName,
		attemptedValue,
		HttpStatusCode.Conflict);
