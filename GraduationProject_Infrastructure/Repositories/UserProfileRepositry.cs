using GraduationProject_Core.Dtos.Auth;
using GraduationProject_Core.Dtos.PersonalExperiance;
using GraduationProject_Core.Dtos.UserProfile;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.AspNetCore.Http;
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
			var p = new ViewProfileDtos()
			{
				UserId = user.Id,
				UserName = user.UserName,
				Email = user.Email,
				Gender = user.Gender,
				Cv = user.Cv,
				GithubLink = user.GithubLink,
				Image = user.Image,
				LinkedInLink = user.LinkedInLink,
				UniversityName = user.University?.Name, // جلب اسم الجامعة
				
				//PersonalExperienceContent = user.PersonalExperience?.Content // جلب وصف التجربة الشخصية
			};
			if (user.PersonalExperience != null)
			{
				p.IsReviewed = user.PersonalExperience.IsReviewed;

				if (!string.IsNullOrEmpty(user.PersonalExperience.Content) && user.PersonalExperience.IsAccepted == true)
				{
					p.PersonalExperienceContent = user.PersonalExperience.Content;
					p.PersonalExperienceId = user.PersonalExperience.PersonalExperienceId;
				}
			}

			return p;
		}
		public async Task<bool> UpdateUserProfileAsync(int id, UpdateProfileDtos dto)
		{
			var user = await dbContext.Users
				.Include(u => u.University)
				.Include(p => p.PersonalExperience)
				.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
			{
				throw new KeyNotFoundException("User not found");
			}
			if (dto.UserName == null)
			{

			user.UserName = user.UserName; // يمكن أن تكون null
			}
			else
			{
				user.UserName = dto.UserName; // يمكن أن تكون null

			}
			if (dto.Gender.HasValue)
			{
				user.Gender = dto.Gender.Value;
			}
			user.GithubLink = dto.GithubLink; // يمكن أن تكون null
			user.LinkedInLink = dto.LinkedInLink; // يمكن أن تكون null

			if (user.University != null)
			{
				user.University.Name = dto.UniversityName; // يمكن أن تكون null
			}

			//if (user.PersonalExperience != null)
			//{
			//	user.PersonalExperience.Content = dto.PersonalExperienceContent; // يمكن أن تكون null
			//}

			// ✅ التعامل مع ملف الـ CV
			if (dto.Cv != null)
			{
				if (!string.IsNullOrEmpty(user.Cv))
				{
					FileHelper.DeleteFile(user.Cv, "Cvs");
				}
				string newCvFileName = FileHelper.UplodeFile(dto.Cv, "Cvs");
				user.Cv = newCvFileName;
			}
			else if (!string.IsNullOrEmpty(dto.OldCvUrl))
			{
				user.Cv = dto.OldCvUrl;
			}
			else
			{
				user.Cv = null;
			}

			// ✅ التعامل مع الصورة
			//اذا تم ارسال صورة جديدة يتم حذف القديمة
			if (dto.Image != null)
			{
				if (!string.IsNullOrEmpty(user.Image))
				{
					FileHelper.DeleteFile(user.Image, "Images");
				}
				//حفظ الصورة الجديدة
				string newFileName = FileHelper.UplodeFile(dto.Image, "Images");
				user.Image = newFileName;
			}
			else if (!string.IsNullOrEmpty(dto.OldImageUrl))
			{
				user.Image = dto.OldImageUrl;
			}
			else
			{
				user.Image = user.Gender == Gender.Male
					? "/default image/Man default image.png"
					: "/default image/women default image.png";
			}



			dbContext.Users.Update(user);
			await dbContext.SaveChangesAsync();
			return true;
		}
		public async Task<List<GetUserByNameDto>> GetUsersByNameAsync(string userName)
		{
			var users = await dbContext.Users
				.Where(u => EF.Functions.Like(u.UserName.ToLower(), userName.ToLower() + "%"))
				.Select(x => new GetUserByNameDto()
				{
					Id=x.Id,
					UserName = x.UserName
				})
				.ToListAsync();
			return users;
		}
		public async Task<IEnumerable<User>> GetSubAdminsByUniversityIdAsync(int universityId)
		{
			// استرجاع المستخدمين الذين لديهم UniversityId مطابق
			var usersInUniversity = dbContext.Users.Where(u => u.UniversityId == universityId).ToList();

			// تصفية المستخدمين بناءً على دور "SubAdmin"
			var subAdmins = new List<User>();
			foreach (var user in usersInUniversity)
			{
				if (await userManager.IsInRoleAsync(user, "SuperAdmin"))
				{
					subAdmins.Add(user);
				}
			}

			return subAdmins;
		}


		//public async Task<bool> UpdateUserProfileAsync(int id, UpdateProfileDtos dto)
		//{
		//	var user =await dbContext.Users
		//		.Include(u=>u.University)
		//		.Include(p=>p.PersonalExperience)
		//		.FirstOrDefaultAsync(u => u.Id == id);
		//	if (user == null)
		//	{
		//		throw new KeyNotFoundException("User not found");
		//	}
		//	if (!string.IsNullOrEmpty(dto.UserName))
		//		user.UserName = dto.UserName;

		//	//if (!string.IsNullOrEmpty(dto.Email)) 
		//	//	user.Email = dto.Email;

		//	if (dto.Gender.HasValue)
		//		user.Gender = dto.Gender.Value;

		//	if (!string.IsNullOrEmpty(dto.GithubLink))
		//		user.GithubLink = dto.GithubLink;

		//	if (!string.IsNullOrEmpty(dto.LinkedInLink))
		//		user.LinkedInLink = dto.LinkedInLink;

		//	if(! string.IsNullOrEmpty(dto.UniversityName)) 
		//		user.University.Name = dto.UniversityName;

		//	if (!string.IsNullOrEmpty(dto.PersonalExperienceContent))
		//		user.PersonalExperience.Content = dto.PersonalExperienceContent;

		//	if (dto.Cv != null)
		//	{
		//		// حذف السيرة الذاتية القديمة إذا كانت موجودة
		//		if (!string.IsNullOrEmpty(user.Cv))
		//		{
		//			FileHelper.DeleteFile(user.Cv, "Cvs");
		//		}

		//		// رفع السيرة الذاتية الجديدة
		//		string newCvFileName = FileHelper.UplodeFile(dto.Cv, "Cvs");
		//		user.Cv = newCvFileName;
		//	}


		//	// تحديث الصورة إذا تم رفع صورة جديدة
		//	if (dto.Image != null)
		//	{
		//		// حذف الصورة القديمة
		//		if (!string.IsNullOrEmpty(user.Image) )

		//		{
		//			FileHelper.DeleteFile(user.Image, "Images");
		//		}

		//		// رفع الصورة الجديدة
		//		string newFileName = FileHelper.UplodeFile(dto.Image, "Images");
		//		user.Image = newFileName;
		//	}
		//	dbContext.Users.Update(user);
		//	await dbContext.SaveChangesAsync();
		//	return true;
		//}


	}
}
