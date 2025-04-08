using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.University
{
     public class CreateUniversityDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile Logo { get; set; }   // صورة الشعار
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Gmail { get; set; }
        [Required]
        public IFormFile ImageName { get; set; }
    }
}
