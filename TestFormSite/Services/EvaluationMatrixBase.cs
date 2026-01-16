using DocumentFormat.OpenXml.Bibliography;
using System.Text.Json.Nodes;

namespace TestFormSite.Services
{
    public interface IEvaluationMatrixHandler
    {
        public void UseMatrix(JsonArray items, JsonArray awnsers);
    }
}
