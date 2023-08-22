using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.ENTITY.Concrete
{
    public class ImageModel
    {
        [DisplayName("Upload Image")]
        public string? FileDetails { get; set; }
        public IFormFile? File { get; set; }
        public string? ImageUrl { get; set; }

    }
}
