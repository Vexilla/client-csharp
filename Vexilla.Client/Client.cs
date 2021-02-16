using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Vexilla.Client
{

    public static class VexillaFeatureTypes
    {
        public const string TOGGLE = "toggle";
        public const string GRADUAL = "gradual";
    }

    public class VexillaFlags
    {
        public Dictionary<string, Dictionary<string, Dictionary<string, JObject>>> environments { get; set; }
    }

    public class VexillaClient
    {
        private string baseUrl;
        private string environment;
        private string customInstanceHash;
        private HttpClient client;

        private JObject flags;

        public VexillaClient(string baseUrl, string environment, string customInstanceHash, HttpClient client)
        {
            this.baseUrl = baseUrl;
            this.environment = environment;
            this.customInstanceHash = customInstanceHash;
            this.client = client;
        }

        public async Task<string> fetchString()
        {
            var result = await client.GetAsync(baseUrl + "/features.json");
            var content = await result.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(content);
            return content;
        }

        public async Task<JObject> fetchFlags(string fileName)
        {
            // fetch json
            var result = await client.GetAsync(
                baseUrl + "/" + fileName
            );

            Console.Write("BAAAAR");

            System.Diagnostics.Debug.WriteLine("FOOOOOOO");
            var content = await result.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(content);

            // parse json
            //VexillaFlags config = JsonConvert.DeserializeObject<VexillaFlags>(content);

            JObject config = JObject.Parse(content);

            return config;
        }

        public void setFlags(JObject flags)
        {
            this.flags = flags;
        }

        public bool should(string featureName) {

            JObject environments = flags.GetValue("environments").Value<JObject>();
            JObject environment = environments.GetValue(this.environment).Value<JObject>();
            JObject featureSet = environment.GetValue("untagged").Value<JObject>();
            JObject feature = featureSet.GetValue(featureName).Value<JObject>();

            string featureType = feature.GetValue("type").Value<string>();

            if(featureType == VexillaFeatureTypes.TOGGLE)
            {
                return feature.GetValue("value").Value<bool>();
            } else if(featureType == VexillaFeatureTypes.GRADUAL)
            {
                int value = feature.GetValue("value").Value<int>();
                float seed = feature.GetValue("seed").Value<float>();
                VexillaHasher hasher = new VexillaHasher(seed);

                return hasher.hashString(customInstanceHash) < value;
            }

            return false;
        }
    }
}
