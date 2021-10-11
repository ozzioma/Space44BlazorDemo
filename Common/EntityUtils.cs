using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Common
{
    public static class EntityUtils
    {
        public static string ToJson(this object @object)
        {
            return JsonConvert.SerializeObject(@object,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.None,
                    //ContractResolver = new  CamelCasePropertyNamesContractResolver(),
                    ContractResolver = new DefaultContractResolver(),
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All
                });
        }


        // public static string ToJsonSystem(this object @object)
        // {
        //     return System.Text.Json.JsonSerializer.Serialize(@object);
        // }
    }
}