using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Team
{
	public class TeamDetailsDto
	{
		public string TeamName { get; set; }
		public string UniversityName { get; set; }
		public string Coach { get; set; }
		public List<string> Participants { get; set; }
	}
}
