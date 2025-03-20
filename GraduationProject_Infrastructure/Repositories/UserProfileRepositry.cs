using GraduationProject_Core.Dtos.PersonalExperiance;
using GraduationProject_Core.Dtos.UserProfile;
using GraduationProject_Core.Helper;
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
		public async Task<bool> UpdateUserProfileAsync(int id, UpdateProfileDtos dto)
		{
			var user =await dbContext.Users
				.Include(u=>u.University)
				.Include(p=>p.PersonalExperience)
				.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
			{
				throw new KeyNotFoundException("User not found");
			}
			if (!string.IsNullOrEmpty(dto.UserName))
				user.UserName = dto.UserName;

			if (!string.IsNullOrEmpty(dto.Email)) 
				user.Email = dto.Email;

			if (dto.Gender.HasValue)
				user.Gender = dto.Gender;

			if (!string.IsNullOrEmpty(dto.GithubLink))
				user.GithubLink = dto.GithubLink;

			if (!string.IsNullOrEmpty(dto.LinkedInLink))
				user.LinkedInLink = dto.LinkedInLink;

			if(! string.IsNullOrEmpty(dto.UniversityName)) 
				user.University.Name = dto.UniversityName;

			if (!string.IsNullOrEmpty(dto.PersonalExperienceContent))
				user.PersonalExperience.Content = dto.PersonalExperienceContent;

			if (!string.IsNullOrEmpty(dto.Cv))
				user.Cv = dto.Cv;


			// تحديث الصورة إذا تم رفع صورة جديدة
			if (dto.Image != null)
			{
				// حذف الصورة القديمة
				if (!string.IsNullOrEmpty(user.Image))
				{
					FileHelper.DeleteFile(user.Image, "Images");
				}

				// رفع الصورة الجديدة
				string newFileName = FileHelper.UplodeFile(dto.Image, "Images");
				user.Image = newFileName;
			}
			dbContext.Users.Update(user);
			await dbContext.SaveChangesAsync();
			return true;
		}
	}
}
