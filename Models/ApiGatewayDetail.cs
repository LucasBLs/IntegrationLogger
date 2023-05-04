using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IntegrationLogger.Models;

public class ApiGatewayDetail
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? HeaderName { get; set; }
    public string? HeaderValue { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid ApiGatewayLogId { get; set; }
    public ApiGatewayLog? ApiGatewayLog { get; set; }
}