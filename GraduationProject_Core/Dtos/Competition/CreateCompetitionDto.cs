using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraduationProject_Core.Models.Competition;

namespace GraduationProject_Core.Dtos.Competition
{
    public class CreateCompetitionDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public CompetitionType Type { get; set; }

        [Required]
        public List<string> Location { get; set; } = new List<string>();
        public DateTime? Time { get; set; }

        public string Description { get; set; }

        public IFormFile? Image { get; set; }
        public IFormFile? Image2 { get; set; }
        public IFormFile? QuestionsPDF {  get; set; } 
    }
}
