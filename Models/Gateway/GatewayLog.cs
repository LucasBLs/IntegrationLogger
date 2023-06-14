using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IntegrationLogger.Models.Gateway;

public class GatewayLog
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? ProjectName { get; set; }
    public string? RequestPath { get; set; }
    public string? HttpMethod { get; set; }
    public string? ClientIp { get; set; }
    public int? StatusCode { get; set; }
    public long? RequestDuration { get; set; } // Tempo de requisição em milissegundos
    [BsonRepresentation(BsonType.String)]
    public DateTimeOffset Timestamp { get; set; }
    public ICollection<GatewayDetail>? Details { get; set; }
}