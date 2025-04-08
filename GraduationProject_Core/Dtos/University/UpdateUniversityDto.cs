using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.University
{
    public class UpdateUniversityDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Url { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gmail { get; set; }
        public IFormFile? Logo { get; set; }   // صورة جديدة للـ Logo
        public IFormFile? ImageName { get; set; } // صورة جديدة للـ ImageName
    }
}
