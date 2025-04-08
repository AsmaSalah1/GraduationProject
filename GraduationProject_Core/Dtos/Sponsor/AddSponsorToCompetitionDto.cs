using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Sponsor
{
    public class AddSponsorToCompetitionDto
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Website { get; set; }

        public IFormFile? Logo { get; set; } // رفع الشعار

       // [Required]
       // public int CompetitionId { get; set; }
    }
}
