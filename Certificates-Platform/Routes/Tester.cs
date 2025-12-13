using Microsoft.AspNetCore.Mvc;

namespace Certificates_Platform.Routes
{
       

    [ApiController, Route("api")]
    public class Tester : Controller
    {
        [HttpGet("test")]
        public IActionResult Test([FromQuery]int t)
        {
            return Ok("API is working!" + t);
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
