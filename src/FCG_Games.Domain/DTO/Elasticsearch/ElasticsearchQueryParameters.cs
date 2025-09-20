namespace FCG_Games.Domain.DTO.Elasticsearch;

public record ElasticsearchQueryParameters(string Term, int StartDocumentPosition = 0, int Size = 10);
