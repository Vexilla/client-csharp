using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("Vexilla.Client.Tests")]
namespace Vexilla.Client
{

    enum VexillaFeatureType
    {
        toggle,
        gradual,
        selective,
    }

    interface VexillaFeatureConfig
    {
        string Type { get; }
    }

    interface VexillaToggleFeature: VexillaFeatureConfig
    {

        int Value { get; }

    }

    interface VexillaGradualFeature : VexillaFeatureConfig
    {

        int Value { get;  }

        float Seed { get; }
    }

    interface VexillaSelectiveFeature : VexillaFeatureConfig
    {
    }

    interface VexillaFeature : IDictionary<string, VexillaFeatureConfig> { }
    interface VexillaFeatureSet : IDictionary<string, VexillaFeature> { }
    interface VexillaEnvironment : IDictionary<string, VexillaFeatureSet> { }

    interface VexillaConfig
    {
        Dictionary<string, VexillaEnvironment> Environments { get; }
    }

    public class VexillaClient
    {
        private string baseUrl;
        private string environment;
        private string customInstanceHash;

        private VexillaConfig flags;

        public VexillaClient(string baseUrl, string environment, string customInstanceHash)
        {
            this.baseUrl = baseUrl;
            this.environment = environment;
            this.customInstanceHash = customInstanceHash;
        }

        public async Task fetchFlags(string fileName)
        {
            // fetch json
            var client = new HttpClient();
            var result = await client.GetAsync(
                baseUrl + "/" + fileName+ ".json"
            );
            System.Diagnostics.Debug.WriteLine(result.Content.ToString());

            // parse json
            VexillaConfig config  = JsonConvert.DeserializeObject<VexillaConfig>(result.Content.ToString());

            // set as flags
            this.flags = config;



        }



        public bool should() {
            return false;
        }
    }
}
