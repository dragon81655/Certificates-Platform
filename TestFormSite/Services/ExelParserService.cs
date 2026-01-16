using Microsoft.AspNetCore.Mvc;
using ServiceRegistration;

namespace TestFormSite.Services
{
    [RegisterAsTransient]
    public class ExelParserService
    {
        public ExelParserService()
        {
               
        }

        public IActionResult ParseExel()
        {
            /*byte[] fileBytes = System.IO.File.ReadAllBytes("path/to/file.xlsx");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");*/
        }
    }
}
