using Newtonsoft.Json;

namespace IntegrationLogger.Extensions;
public static class SerializeIndentedObjectExtension
{
    public static string SerializeIndentedObject(this object? content)
    {
        return JsonConvert.SerializeObject(content, Formatting.Indented, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
    }
}