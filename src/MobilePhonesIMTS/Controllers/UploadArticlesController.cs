using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using MobilePhonesIMTS.Models;
using MobilePhonesIMTS.Data;
using System.Security.Claims;

namespace MobilePhonesIMTS.Controllers
{
    public class UploadArticlesController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;

        public UploadArticlesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
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
            //long size = files.Sum(f => f.Length);

            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var filePath = uploads;

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, formFile.FileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);

                        ViewData["FilePath"] = filePath +  "\\" + formFile.FileName;
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            //return Ok(new { count = files.Count, size, filePath });
            

            //create
            return View("~/Views/UploadArticles/Success.cshtml");
        }

        [Authorize]
        [HttpPost("PostArticlesForm")]
        public async Task<IActionResult> PostOneArticleForm(IFormFile specificFile)
        {
            //long size = files.Sum(f => f.Length);

            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var filePath = uploads;

                if (specificFile.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, specificFile.FileName), FileMode.Create))
                    {
                        await specificFile.CopyToAsync(fileStream);

                        ViewData["FilePath"] = filePath + "\\" + specificFile.FileName;
                    }
                }
           

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            //return Ok(new { count = files.Count, size, filePath });


            //create
            return View("~/Views/UploadArticles/Success.cshtml");
        }

        [Authorize]
        [HttpPost("PostArticleSave")]
        public async Task<IActionResult> PostArticleSave(ArticleViewModel model, IFormFile specificFile)
        {
            if (ModelState.IsValid)
            {
                var uploadsDir = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

                var articleToAdd = new Article();
                articleToAdd.Title = model.Title;
                articleToAdd.Category = model.Category;
                articleToAdd.DatePublished = DateTime.Now;
                articleToAdd.City = model.City;
                articleToAdd.ArticleAbstract = model.ArticleAbstract;

                ClaimsPrincipal currentUser = User;
                articleToAdd.UserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                //if (fileSelect.Length > 0)
                //{
                //    using (var fileStream = new FileStream(Path.Combine(uploadsDir, fileSelect.FileName), FileMode.Create))
                //    {
                //        await fileSelect.CopyToAsync(fileStream);
                //    }
                //    articleToAdd.SystemPath = uploadsDir;
                //}
                //else
                //{
                //    articleToAdd.SystemPath = "";
                //}


                    if (specificFile.Length > 0)
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploadsDir, specificFile.FileName), FileMode.Create))
                        {
                            await specificFile.CopyToAsync(fileStream);
                            
                            articleToAdd.SystemPath = specificFile.FileName;
                            ViewData["FilePath"] = uploadsDir + "\\" + specificFile.FileName;
                        }
                    }
                

                _context.Add(articleToAdd);
                await _context.SaveChangesAsync();

            }
            //create
            return View("~/Views/UploadArticles/Success.cshtml");
        }

    }
}