using IntegrationLogger.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IntegrationLogger.Models;

public class IntegrationLog
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? IntegrationName { get; set; }
    public string? Message { get; set; }
    public string? ExternalSystem { get; set; }
    public string? SourceSystem { get; set; }
    [BsonRepresentation(BsonType.String)]
    public DateTimeOffset Timestamp { get; set; }
    public ICollection<IntegrationDetail>? Details { get; set; }
}
