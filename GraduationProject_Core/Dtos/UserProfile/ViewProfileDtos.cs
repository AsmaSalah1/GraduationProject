using GraduationProject_Core.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.UserProfile
{
	public class ViewProfileDtos
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string? Image { get; set; }
		public Gender? Gender { get; set; }
		public string? GithubLink { get; set; }
		public string? LinkedInLink { get; set; }
		public string? Cv { get; set; }
		public string? UniversityName { get; set; }
		public string? PersonalExperienceContent { get; set; }

	}
}
