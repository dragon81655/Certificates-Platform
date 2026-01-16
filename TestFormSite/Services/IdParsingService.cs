using Microsoft.AspNetCore.Mvc;
using ServiceRegistration;
using System.Text.Json.Nodes;

namespace TestFormSite.Services
{

    [RegisterAsScoped]
    public class FormParsingService
    {
        private Dictionary<string, LabelData> labels = new Dictionary<string, LabelData>();

        public void Init(JsonArray structure)
        {

            foreach (JsonObject t in structure)
            {
                string id = t["id"].ToString();
                string label = t["label"].ToString();
                labels.Add(id,
                    new LabelData
                    {
                        label = label
                    });
            }
        }

        public LabelData GetLabel(string id)
        {
            return labels[id];
        }

        public class LabelData
        {
            public int exelColumn = -1;
            public string label = "";
        }
    }
}
