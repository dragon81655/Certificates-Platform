using Microsoft.AspNetCore.Mvc;
using Certificates_Platform.Services;
namespace Certificates_Platform.Controllers
{
       

    [ApiController, Route("api")]
    public class Tester : Controller
    {
        CertificateJobOrchestratorService service;
        public Tester(CertificateJobOrchestratorService service)
        {
            this.service = service;
        }
        [HttpGet("test")]
        public IActionResult Test([FromQuery]int t)
        {
            service.AddJob(0);
            return Ok("API is working!" + t);
        }
        [HttpPost("upload")]
        public IActionResult UploadPdf([FromForm] IFormFile file)
        {
            Console.WriteLine(file.FileName);
            return Ok("Worked!");
        }
        [HttpGet("certificate")]
        public IActionResult Test()
        {
            return Ok("API is working!");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
