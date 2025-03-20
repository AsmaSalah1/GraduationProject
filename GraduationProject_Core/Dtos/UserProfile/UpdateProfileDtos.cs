using GraduationProject_Core.Dtos.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.UserProfile
{
	public class UpdateProfileDtos
	{
		public string? UserName { get; set; }
		public string? Email { get; set; }
	//	public string? ImageName { get; set; }
		public IFormFile? Image { get; set; } // استقبال الصورة عند التحديث
		public Gender? Gender { get; set; }
		public string? GithubLink { get; set; }
		public string? LinkedInLink { get; set; }
		public string? Cv { get; set; }
		public string? UniversityName { get; set; }
		public string? PersonalExperienceContent { get; set; }
	}
}
