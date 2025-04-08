using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Sponsor
{
    public class UpdateSponsorDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }
        public IFormFile? Logo { get; set; } // الصورة الجديدة (اختياري)

    }
}
