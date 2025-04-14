using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class Team
	{
		[Key]
		public int TeamId { get; set; }
		[ForeignKey(nameof(User))]
		public int UserId { get; set; }
		public string TeamName { get; set; }
		public string UniversityName { get; set; }
		public string? Coach { get; set; }
		// العلاقات
		public ICollection<TeamCompetition> TeamsCompetitions { get; set; } = new HashSet<TeamCompetition>();
		public ICollection<TeamParticipant> TeamsParticipant { get; set; } = new HashSet<TeamParticipant>();
	}
}
