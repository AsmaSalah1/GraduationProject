using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class University
	{
		[Key]
		public int UniversityId { get; set; }
		public string Name { get; set; }
		public string Logo { get; set; }
		public string Description { get; set; }
		public string? Location { get; set; }
		public string? ImageName { get; set; }
		public string? Url { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Gmail { get; set; }
		public ICollection<User> Users { get; set; }=new HashSet<User>();
		public ICollection<UniversityCompetition>? UniversityCompetitions { get; set; }=new HashSet<UniversityCompetition>();
	   public ICollection<UniversityImages> UniversityImages { get; set; }=new HashSet<UniversityImages>();
	}
}
