using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationProject_Core.Dtos.Auth;

namespace GraduationProject_Core.Models
{
	public class User:IdentityUser<int>
	{

		[ForeignKey(nameof(University))]
		public int? UniversityId { get; set; }
		//	public string? UniversityName { get; set; } بحطها في ال DTOs
		[ForeignKey(nameof(PersonalExperience))]
		public int? PersonalExperienceId { get; set; }
		public string? Image { get; set; }
		public Gender? Gender { get; set; }
		public string? GithubLink { get; set; }
		public string? LinkedInLink { get; set; }
		public string? Cv { get; set; }
		public ICollection<Post>? Posts { get; set; }
		public University? University { get; set; }
		public PersonalExperience? PersonalExperience { get; set; }
		public ICollection<QAA>? QAAs { get; set; }=new HashSet<QAA>();
		public ICollection<Rule>? Rules { get; set; }=new HashSet<Rule>();
	}
}
