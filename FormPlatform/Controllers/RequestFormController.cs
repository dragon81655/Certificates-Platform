using FormPlatform.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
 

namespace FormPlatform.Controllers
{
    [ApiController, Route("api/forms")]
    public class RequestFormController : ControllerBase
    {

        FormsDataCollectionService _formsDataCollectionService;

        public RequestFormController(FormsDataCollectionService formsDataCollectionService)
        {
            _formsDataCollectionService = formsDataCollectionService;
        }

        [HttpGet("full_data")]
        public async Task<IActionResult> GetData()
        {
            FormData data = await _formsDataCollectionService.GetFormData();



            return Ok("In development, nothing sent");
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok("Request Form Service is running.");
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestEndpoint()
        {
            FormData t = await _formsDataCollectionService.GetFormData();
            Console.WriteLine(t.awnsers[0]);
            return Ok("Test endpoint is working.");
        }
    }
}
