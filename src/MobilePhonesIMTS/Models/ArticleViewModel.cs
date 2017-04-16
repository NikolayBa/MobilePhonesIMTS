using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobilePhonesIMTS.Models
{
    public class ArticleViewModel
    {
        public string Title { get; set; }

        public string Category { get; set; }

        public string City { get; set; }

        public string ArticleAbstract { get; set; }

        public IFormFile ActualFile { get; set; }
    }
}
