using GraduationProject_Core.Dtos.UserProfile;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Infrastructure.Repositories
{
	public class UserProfileRepositry : IUserProfileRepositry
	{
		private readonly UserManager<User> userManager;
		private readonly ApplicationDbContext dbContext;

		public UserProfileRepositry(UserManager<User> userManager,ApplicationDbContext dbContext)
		{
			this.userManager = userManager;
			this.dbContext = dbContext;
		}

		public async Task<ViewProfileDtos> GetUserProfileAsync(int id)
		{
			var user = await dbContext.Users
				.Include(u=>u.University)
				.Include(p=>p.PersonalExperience)
				.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null) {
				throw new KeyNotFoundException("User not found");
			}
			return new ViewProfileDtos()
			{
				UserName = user.UserName,
				Email = user.Email,
				Gender = user.Gender,
				Cv = user.Cv,
				GithubLink = user.GithubLink,
				Image = user.Image,
				LinkedInLink = user.LinkedInLink,
				UniversityName = user.University?.Name, // جلب اسم الجامعة
				PersonalExperienceContent = user.PersonalExperience?.Content // جلب وصف التجربة الشخصية
			};
		}

		public Task<bool> UpdateUserProfileAsync(int id, UpdateProfileDtos updateDto)
		{
			throw new NotImplementedException();
		}
	}
}
