using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.UniversityImage
{
    public class AddUniversityImageDto
    {
        [Required]
        public List<IFormFile> Images { get; set; }
    }
}
