namespace FCG_Games.Domain.DTO.Elasticsearch.ElasticsearchDocuments;

public class GameDocument
{
	public Guid Id { get; set; }
	public required string Title { get; set; }
	public string? Description { get; set; }
	public required string Developer { get; set; }
	public required string Publisher { get; set; }
	public DateOnly ReleaseDate { get; set; }
}
