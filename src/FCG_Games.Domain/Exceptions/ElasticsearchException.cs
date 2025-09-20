using FCG_Games.Domain.Enums;
using System.Net;

namespace FCG_Games.Domain.Exceptions;

public class ElasticsearchException(ElasticsearchOperation operation, string? indexName = null, string? documentId = null) 
	: AppException("An error occurred during a Elasticsearch operation", ErrorCode.InfrastructureError, HttpStatusCode.InternalServerError)
{
	public string Operation { get; set; } = operation.ToString();
	public string? IndexName { get; set; } = indexName;
	public string? DocumentId { get; set; } = documentId;
}
