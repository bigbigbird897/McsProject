using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Collections;

namespace CoreServiceLib.Tools
{
    public class JsonTool
    {
        public static string GetJsonValue(JEnumerable<JToken> jToken, string key)
        {
            IEnumerator imp = jToken.GetEnumerator();
            while (imp.MoveNext())
            {
                JToken jc = (JToken)imp.Current;
                if (jc.HasValues)
                {
                    if (jc is JObject || ((JProperty)jc).Value is JObject)
                    {
                        //判断是否为空，若为空丢弃掉
                        if (jc.First.Any())
                        {
                            var impv = GetJsonValue(jc.Children(), key);
                            if (impv != null) return impv;
                        }
                    }
                    else
                    {
                        if (((JProperty)jc).Name == key)
                        {
                            return ((JProperty)jc).Value.ToString();
                        }
                    }
                }
            }
            return null;
        }

        public static T JsonObjectToObject<T>(JObject asobject)
        {
            if (asobject == null) return default;
            var json = asobject.ToString();
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        public static string JObjectToJsonString(JObject asobject)
        {
            if (asobject == null) return "";
            return asobject.ToString();
        }

        public static JObject JsonStringToJObject(string json)
        {
            return (JObject)JsonConvert.DeserializeObject(json);
        }


        public static JObject ObjectToJsonObject<T>(T mobj)
        {
            var obj = JObject.FromObject(mobj);
            return obj;
        }

        public static T JsonToObject<T>(string json) where T : new()
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ObjectToJson<T>(T obj) where T : new()
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}