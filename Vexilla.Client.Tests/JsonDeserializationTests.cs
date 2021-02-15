using Newtonsoft.Json;
using Xunit;

namespace Vexilla.Client.Tests
{
    public class JsonDeserializationTests
    {
        private string exampleConfig = @"{""environments"":{""test"":{""untagged"":{""db1"":{""value"":false,""type"":""toggle""}}}}}";

        [Fact]
        public void systemTextProducesSameAsNewtonsoft()
        {
            VexillaConfig newtonsoftConfig = JsonConvert.DeserializeObject<VexillaConfig>(exampleConfig);

            Assert.IsType<VexillaConfig>(newtonsoftConfig.ToString());
        }
    }
}
