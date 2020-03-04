using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace React_Frontend.Controllers
{
    internal class JSON
    {
        private static DefaultContractResolver contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy
            {
                OverrideSpecifiedNames = false
            }
        };
        internal static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }
        internal static object Deserialize(string obj)
        {
            return JsonConvert.DeserializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }

    }
}
