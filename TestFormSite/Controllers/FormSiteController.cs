using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceRegistration;
using System.Text.Json.Nodes;
using TestFormSite.Services;

namespace TestFormSite.Controllers
{
    [ApiController, Route("api/forms")]
    public class FormSiteController:ControllerBase
    {
        FormParsingService service;
        private string key;

        private string link;

        public FormSiteController(FormParsingService service, IOptions<FormSiteData> options)
        {
            this.service = service;
            link = options.Value.link;
            key = options.Value.key;
        }

        [HttpGet("test")]
        public async Task<IActionResult> GetStatus()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", key);

            HttpResponseMessage t =await client.GetAsync(link+"results");
            JsonObject objeto = JsonNode.Parse(await t.Content.ReadAsStringAsync()).AsObject();

            HttpResponseMessage b = await client.GetAsync(link+"items");
            Console.WriteLine(await b.Content.ReadAsStringAsync());
            JsonArray t2 = objeto["results"].AsArray();

            service.Init(JsonNode.Parse(await b.Content.ReadAsStringAsync())["items"].AsArray());
            foreach (JsonObject t3 in t2)
            {
                Console.WriteLine(t3.ToString());
            }
            return Ok("Service is running");
        }


    }
}
