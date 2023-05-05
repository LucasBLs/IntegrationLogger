using IntegrationLogger.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IntegrationLogger.Models;

public class ApiGatewayDetail
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DetailType Type { get; set; }
    public string? Message { get; set; }
    public string? Content { get; set; }
    [BsonRepresentation(BsonType.String)]
    public DateTimeOffset Timestamp { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid ApiGatewayLogId { get; set; }
    public ApiGatewayLog? ApiGatewayLog { get; set; }
}
