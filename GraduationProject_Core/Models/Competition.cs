using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class Competition
	{
		[Key]
		public int CompetitionId { get; set; }
		public string Name { get; set; }
	//	public string Type { get; set; }
		public CompetitionType Type { get; set; }
		public string Location { get; set; }
		public DateTime? Time { get; set; }
		public string Description { get; set; }
		public string? Image { get; set; }
		public ICollection<UniversityCompetition> UniversityCompetitions { get; set; } = new HashSet<UniversityCompetition>();
		public ICollection<SponsorComptiition>? SponsorCompetition { get; set; }=new HashSet<SponsorComptiition>();
		public ICollection<TeamCompetition> TeamCompetitions { get; set; }=new HashSet<TeamCompetition>();
		public ICollection<CompetitionImages> CompetitionImages { get; set; }=new HashSet<CompetitionImages>();
		public enum CompetitionType
		{
			Local,
			PCPC
		}
	}

}
