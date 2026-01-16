using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using ServiceRegistration;
namespace FormPlatform.Services
{
    [RegisterAsTransient]
    public class FormsDataCollectionService
    {

        private string key;
        private string link;

        private Dictionary<string, string> dic;

        public FormsDataCollectionService(IOptions<FormSiteData> config)
        {
            key = config.Value.key;
            link = config.Value.link;
        }

        public async Task<FormData> GetFormData()
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", key);

            HttpResponseMessage r = await client.GetAsync(link + "results");
            JsonArray results = JsonNode.Parse(await r.Content.ReadAsStringAsync())["results"].AsArray();

            HttpResponseMessage i = await client.GetAsync(link + "items");
            JsonArray labels = JsonNode.Parse(await i.Content.ReadAsStringAsync())["items"].AsArray();
            dic = CreateLabelDic(labels);
            return new FormData(dic, results);
        }

        private Dictionary<string, string> CreateLabelDic(JsonArray jsonArray)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in jsonArray)
            {
                var obj = item.AsObject();
                dic.Add(obj["id"].ToString(), obj["label"].ToString());
            }
            return dic;
        }

        private List<Dictionary<string, string>>
    }

    public struct FormData
    {
        public Dictionary<string, string> labels;
        public JsonElement[] awnsers;

        public FormData(Dictionary<string, string> dic, JsonArray awnsers)
        {
            this.labels = dic;
            this.awnsers = JsonSerializer.Deserialize<JsonElement[]>(awnsers);
        }
    }
}
