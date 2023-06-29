using IntegrationLogger.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IntegrationLogger.Models.Integration;

public class IntegrationItem
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? ItemIdentifier { get; set; }
    public LogLevel ItemStatus { get; set; }
    public string? Message { get; set; }
    public string? Content { get; set; } = string.Empty;
    [BsonRepresentation(BsonType.String)]
    public DateTimeOffset Timestamp { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid IntegrationDetailId { get; set; }
    public IntegrationDetail? IntegrationDetail { get; set; }
}