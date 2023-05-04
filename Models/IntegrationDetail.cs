using IntegrationLogger.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IntegrationLogger.Models;

public class IntegrationDetail
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    // public ActionType ActionType { get; set; }
    // public string? EntityName { get; set; }
    public IntegrationStatus Status { get; set; }
    public string? DetailIdentifier { get; set; }
    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }
    [BsonRepresentation(BsonType.String)]
    public DateTimeOffset Timestamp { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid IntegrationLogId { get; set; }
    public IntegrationLog? IntegrationLog { get; set; }
    public ICollection<IntegrationItem>? Items { get; set; }
}