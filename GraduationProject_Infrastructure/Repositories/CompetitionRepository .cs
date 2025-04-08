using GraduationProject_Core.Dtos.Competition;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static GraduationProject_Core.Models.Competition;
using GraduationProject_Core.Dtos.CompetitionImage;
using Microsoft.AspNetCore.Http;
namespace GraduationProject_Infrastructure.Repositories
{
	public class CompetitionRepository : ICompetitionRepository
	{
		private readonly ApplicationDbContext dbContext;

		public CompetitionRepository(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<bool> AddCompetitionToUniversityAsync(CreateCompetitionDto dto, int universityId, int userId)
		{
			// رفع الصور كما في الكود السابق
			string newImageName = null;
			string newImageName2 = null;

			if (dto.Image != null)
			{
				newImageName = FileHelper.UplodeFile(dto.Image, "Images");
			}

			if (dto.Image2 != null)
			{
				newImageName2 = FileHelper.UplodeFile(dto.Image2, "Images");
			}

			var competition = new Competition
			{
				Name = dto.Name,
				Type = dto.Type,
				Location = dto.Location,
				Time = dto.Time,
				Description = dto.Description,
				Image = newImageName,
				Image2 = newImageName2
			};

			await dbContext.Competitions.AddAsync(competition);
			await dbContext.SaveChangesAsync();

			// ربط المسابقة بالجامعة باستخدام جدول UniversityCompetition
			var universityCompetition = new UniversityCompetition
			{
				UniversityId = universityId,
				CompetitionID = competition.CompetitionId,
				DateAndTime = DateTime.Now
			};

			await dbContext.UniversityCompetitions.AddAsync(universityCompetition);
			await dbContext.SaveChangesAsync();

			return true;
		}
		public async Task<Competition> GetLastCompetitionByUniversityIdAsync(int universityId)
		{
			// العثور على آخر مسابقة مرتبطة بالجامعة
			var competitionId = await dbContext.UniversityCompetitions
												.Where(uc => uc.UniversityId == universityId)
												.OrderByDescending(uc => uc.DateAndTime)
												.Select(uc => uc.CompetitionID)
												.FirstOrDefaultAsync();

			// استرجاع التفاصيل الخاصة بالمسابقة
			var competition = await dbContext.Competitions
											  .FirstOrDefaultAsync(c => c.CompetitionId == competitionId);

			return competition;  // إذا لم توجد مسابقة ستُرجع null
		}
		public async Task<List<Competition>> GetCompetitionsByUniversityIdAsync(int universityId)
		{
			var competitionIds = await dbContext.UniversityCompetitions
												 .Where(uc => uc.UniversityId == universityId)
												 .Select(uc => uc.CompetitionID)
												 .ToListAsync();

			var competitions = await dbContext.Competitions
											  .Where(c => competitionIds.Contains(c.CompetitionId))
											  .ToListAsync();

			return competitions;  // إرجاع قائمة المسابقات
		}

		/*
        public async Task<bool> AddCompetitionAsync(CreateCompetitionDto dto, int userId)
        {
            var competition = new Competition
            {
                Name = dto.Name,
                Type = dto.Type,
                Location = dto.Location,
                Time = dto.Time,
                Description = dto.Description,
                Image = dto.Image
            };


            await dbContext.Competitions.AddAsync(competition);
            await dbContext.SaveChangesAsync();
            return true;
        }*/
		public async Task<bool> AddCompetitionAsync(CreateCompetitionDto dto, int userId)
		{
			// قم بتحديد صور المسابقة
			string newImageName = null;
			string newImageName2 = null;

			// تحقق إذا كان هناك صورة جديدة تم رفعها
			if (dto.Image != null)
			{
				// رفع الصورة الأولى
				newImageName = FileHelper.UplodeFile(dto.Image, "Images");
			}

			// تحقق إذا كانت هناك صورة ثانية تم رفعها
			if (dto.Image2 != null)
			{
				// رفع الصورة الثانية
				newImageName2 = FileHelper.UplodeFile(dto.Image2, "Images");
			}

			// إنشاء الكائن المسابقة (Competition)
			var competition = new Competition
			{
				Name = dto.Name,
				Type = dto.Type,
				Location = dto.Location, // تخزين المواقع
				Time = dto.Time,
				Description = dto.Description,
				Image = newImageName, // صورة الأولى
				Image2 = newImageName2 // صورة ثانية
			};

			// إضافة المسابقة إلى قاعدة البيانات
			await dbContext.Competitions.AddAsync(competition);
			await dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteCompetitionAsync(int competitionId, int userId)
		{
			var competition = await dbContext.Competitions.FindAsync(competitionId);
			if (competition == null)
			{
				return false;
			}

			dbContext.Competitions.Remove(competition);
			await dbContext.SaveChangesAsync();
			return true;
		}
		//استرجاع كل المسابقات


		public async Task<List<Competition>> GetAllCompetitionsAsync()
		{
			return await dbContext.Competitions
		 .Select(c => new Competition
		 {
			 Name = c.Name,
			 Image = c.Image
		 })
		 .ToListAsync();

		}
		public async Task<List<Competition>> GetCompetitionsByTypeAsync(CompetitionType type)
		{
			return await dbContext.Competitions
				.Where(c => c.Type == type) // فلترة حسب نوع المسابقة
				.Select(c => new Competition
				{
					Name = c.Name,
					Image = c.Image
				})
				.ToListAsync();
		}
		public async Task<bool> DeleteCompetitionAsync(int competitionId)
		{
			var competition = await dbContext.Competitions.FindAsync(competitionId);

			if (competition == null)
			{
				return false; // المسابقة غير موجودة
			}

			dbContext.Competitions.Remove(competition);
			await dbContext.SaveChangesAsync();

			return true; // تم الحذف بنجاح
		}


		public async Task<bool> UpdateCompetitionAsync(int competitionId, UpdateCompetitionDto dto)
		{
			var competition = await dbContext.Competitions.FindAsync(competitionId);

			if (competition == null)
			{
				return false; // المسابقة غير موجودة
			}

			// تعديل الاسم
			//competition.Name = dto.Name ?? competition.Name;
			if (!string.IsNullOrEmpty(dto.Name))
			{
				competition.Name = dto.Name;
			}
			// إذا كان هناك صورة جديدة تم رفعها
			if (dto.Image != null)
			{
				// رفع الصورة الجديدة باستخدام دالة رفع الصور المساعدة
				string newImageName = FileHelper.UplodeFile(dto.Image, "Images");
				competition.Image = newImageName;
			}

			// حفظ التعديلات في قاعدة البيانات
			dbContext.Competitions.Update(competition);
			await dbContext.SaveChangesAsync();

			return true; // تم التعديل بنجاح
		}
		public async Task<Competition> GetCompetitionByNameAsync(string competitionName)
		{
			// البحث عن المسابقة باستخدام اسمها
			var competition = await dbContext.Competitions
											 .FirstOrDefaultAsync(c => c.Name == competitionName);

			return competition;  // إذا كانت المسابقة موجودة سترجعها، إذا لم تكن موجودة سترجع null
		}
		/*;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;*/
		public async Task<Competition> GetCompetitionByIdAsync(int competitionId)
		{
			var competition = await dbContext.Competitions
											  .FirstOrDefaultAsync(c => c.CompetitionId == competitionId);

			return competition;  // إذا كانت المسابقة موجودة سترجعها، إذا لم تكن موجودة سترجع null
		}

		public async Task<bool> UpdateDescriptionAsync(int competitionId, string description)
		{
			var competition = await dbContext.Competitions.FindAsync(competitionId);

			if (competition == null)
			{
				return false;
			}

			competition.Description = description;

			dbContext.Competitions.Update(competition);
			await dbContext.SaveChangesAsync();

			return true;
		}
		public async Task<bool> UpdateLocationAsync(int competitionId, List<string> locations)
		{
			var competition = await dbContext.Competitions.FindAsync(competitionId);

			if (competition == null)
			{
				return false;
			}

			competition.Location = locations;

			dbContext.Competitions.Update(competition);
			await dbContext.SaveChangesAsync();

			return true;
		}

		public async Task<bool> UpdateImage2Async(int competitionId, IFormFile image2)
		{
			var competition = await dbContext.Competitions.FindAsync(competitionId);

			if (competition == null)
			{
				return false;
			}

			// رفع الصورة باستخدام FileHelper (تأكد من وجود هذه الدالة أو أن تقوم بإنشاء واحدة للرفع)
			string newImageName2 = FileHelper.UplodeFile(image2, "Images");

			competition.Image2 = newImageName2;

			dbContext.Competitions.Update(competition);
			await dbContext.SaveChangesAsync();

			return true;
		}

	}
}
