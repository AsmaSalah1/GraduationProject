using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class TeamCompetition
	{
		public int TeamId { get; set; }
		public int CompetitionID { get; set; }
		public int year { get; set; }
		public int Ranking { get; set; }
		public Team Team { get; set; }
		public Competition Competition { get; set; }
	}
}
