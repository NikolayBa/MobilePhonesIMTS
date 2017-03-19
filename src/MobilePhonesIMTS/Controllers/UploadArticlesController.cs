using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace MobilePhonesIMTS.Controllers
{
    public class UploadArticlesController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadArticlesController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> PostArticle(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var filePath = uploads;

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, formFile.FileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            //return Ok(new { count = files.Count, size, filePath });
            ViewData["FilePath"] = filePath;
            return View("~/Views/UploadArticles/Success.cshtml");
        }

    }
}