using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntegrationLogger.Extensions;
public static class SerializeIndentedObjectExtension
{
    public static string SerializeIndentedObject(this object? content)
    {
        // Se o conteúdo já é uma string, tenta interpretar como JSON.
        // Se for JSON válido, retorna a string identada.
        // Se não for JSON válido, retorna a string original.
        if (content is string str)
        {
            try
            {
                var obj = JToken.Parse(str);
                return obj.ToString(Formatting.Indented); // Identação aqui
            }
            catch (JsonReaderException)
            {
                return str;
            }
        }

        // Se o conteúdo não é uma string, tenta serializar para JSON.
        // Se a serialização falhar, retorna uma representação em string do objeto.
        try
        {
            return JsonConvert.SerializeObject(content, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        catch (Exception)
        {
            return content?.ToString() ?? "null";
        }
    }
}
