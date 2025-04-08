using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraduationProject_Core.Models.Competition;

namespace GraduationProject_Core.Dtos.Competition
{
    public class UpdateCompetitionDto
    {
		public string? Name { get; set; }
		public IFormFile? Image { get; set; }
		public string? Description { get; set; }
		public string? Location { get; set; }
		public DateTime? Time { get; set; }
		public List<IFormFile>? CompititionsImages { get; set; }
		//public string? Name { get; set; }
		//public IFormFile? Image { get; set; }
	}
}
