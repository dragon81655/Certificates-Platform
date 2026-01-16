using Microsoft.AspNetCore.Mvc;
using Certificates_Platform.Services;
using Microsoft.Extensions.Options;
namespace Certificates_Platform.Controllers
{
    [ApiController, Route("api/certs")]
    public class CertificateRequestsController : ControllerBase
    {
        CertificateJobOrchestratorService certificateJobOrchestrator;
        private static int cookieLifetime;
        public CertificateRequestsController(CertificateJobOrchestratorService certificateJobOrchestrator, IOptions<GenerationSettings> settings)
        {
            this.certificateJobOrchestrator = certificateJobOrchestrator;
            cookieLifetime = settings.Value.folderLifeTimeMinutes;
        }
        [HttpGet("checkid")]
        public IActionResult CheckForId()
        {
            Request.Cookies.TryGetValue("jobID", out string? jobID);
            if (jobID == null)
            {
                return NotFound(new { message = "No job in progress" });
            }

            int id = int.Parse(jobID);
            if (certificateJobOrchestrator.IsJobCompleted(id))
            {
                return Ok(new { message = "Job completed"});
            }
            else
            {
                return Ok(new { message = "Job still in progress"});
            }

        }


        [HttpGet("checkup")]
        public IActionResult CheckUp()
        {
            Request.Cookies.TryGetValue("jobID", out string? jobID);
            if (jobID == null)
            {
                return NotFound(new { message = "No job in progress" });
            }

            int id = int.Parse(jobID);
            if (certificateJobOrchestrator.IsJobCompleted(id))
            {
                return Ok(new { message = "Job completed", jobID = id });
            }
            else
            {
                return Ok(new { message = "Job still in progress", jobID = id });
            }
        }


        [HttpPost("upload")]
        public IActionResult RequestUpload([FromForm] IFormFile pdf, [FromForm] IFormFile exel)
        {
            Console.WriteLine("Reached!");
            int t = certificateJobOrchestrator.CreateFilesAndID(pdf, exel);

            IActionResult actionResult = Ok(new { message = "All went well" });
            Response.Cookies.Append("jobID", t.ToString(),
               new CookieOptions
               {
                   HttpOnly = true,
                   SameSite = SameSiteMode.Strict,
                   Expires = DateTimeOffset.UtcNow.AddMinutes(cookieLifetime),
                   IsEssential = true
               });
            return actionResult;
        }
    }
}
