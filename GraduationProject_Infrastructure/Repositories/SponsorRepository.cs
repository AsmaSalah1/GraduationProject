using GraduationProject_Core.Dtos.Sponsor;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using GraduationProject_Core.Dtos.Competition;
using GraduationProject_Core.Dtos.CompetitionImage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace GraduationProject_Infrastructure.Repositories
{
    public class SponsorRepository: ISponsorRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SponsorRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // إضافة Sponsor وربطه بالمسابقة
        public async Task<bool> AddSponsorToCompetitionAsync(int competitionId, AddSponsorToCompetitionDto dto)
        {
            // التحقق من وجود المسابقة
            var competition = await dbContext.Competitions.FindAsync(competitionId);
            if (competition == null)
            {
                return false; // المسابقة غير موجودة
            }

            string logoFileName = null;
            if (dto.Logo != null)
            {
                logoFileName = FileHelper.UplodeFile(dto.Logo, "Images"); // رفع الشعار
            }

            // إضافة الـ Sponsor إلى قاعدة البيانات
            var sponsor = new Sponsor
            {
                Name = dto.Name,
                Description = dto.Description,
                Website = dto.Website,
                Logo = logoFileName // حفظ اسم الملف للصورة
            };

            // إضافة الداعم إلى قاعدة البيانات
            await dbContext.Sponsors.AddAsync(sponsor);
            await dbContext.SaveChangesAsync(); // حفظ الداعم الجديد

            // ربط الداعم بالمسابقة
            var sponsorCompetition = new SponsorComptiition
            {
                SponsorID = sponsor.SponsorID,
                CompetitionID = competitionId // ربط المسابقة باستخدام الـ competitionId
            };

            // إضافة الرابط بين الداعم والمسابقات في الجدول الوسيط
            await dbContext.SponsorComptiitions.AddAsync(sponsorCompetition);
            await dbContext.SaveChangesAsync(); // حفظ الرابط بين الداعم والمسابقات

            return true; // تم إضافة الداعم وربطه بالمسابقة بنجاح
        }

        public async Task<List<Sponsorl>> GetSponsorsByCompetitionIdAsync(int competitionId)
        {
            // استرجاع الداعمين المرتبطين بالمسابقة دون إرجاع SponsorID
            var sponsors = await dbContext.SponsorComptiitions
                .Where(sc => sc.CompetitionID == competitionId)
                .Include(sc => sc.Sponsor) // استرجاع الداعم المرتبط بالمسابقة
                .Select(sc => new Sponsorl
                {
                    Id=sc.SponsorID,
                    Name = sc.Sponsor.Name,
                    Description = sc.Sponsor.Description,
                    Website = sc.Sponsor.Website,
                    Logo = sc.Sponsor.Logo // إرجاع صورة الشعار أو أي تفاصيل أخرى
                })
                .ToListAsync();

            return sponsors;
        }



        public async Task<bool> RAddSponsorToCompetitionAsync(int competitionId, int sponsorId)
        {
            var competition = await dbContext.Competitions.FindAsync(competitionId);
            if (competition == null)
            {
                return false; // المسابقة غير موجودة
            }

            var sponsor = await dbContext.Sponsors.FindAsync(sponsorId);
            if (sponsor == null)
            {
                return false; // السبونسور غير موجود
            }

            var sponsorCompetition = new SponsorComptiition
            {
                SponsorID = sponsor.SponsorID,
                CompetitionID = competitionId
            };
            await dbContext.SponsorComptiitions.AddAsync(sponsorCompetition);
            await dbContext.SaveChangesAsync();

            return true; // تم ربط السبونسور بالمسابقة
        }

        public async Task<List<SponsorDto>> GetAllSponsorsAsync()
        {
            // استرجاع كل السبونسورز مع الـ SponsorId
            var sponsors = await dbContext.Sponsors
                                           .Select(s => new SponsorDto
                                           {
                                               SponsorId = s.SponsorID,
                                               Name = s.Name,
                                           })
                                           .ToListAsync();

            return sponsors;
        }

        public async Task<bool> UpdateSponsorAsync(int sponsorId, UpdateSponsorDto dto)
        {
            var sponsor = await dbContext.Sponsors.FindAsync(sponsorId);
            if (sponsor == null)
            {
                return false; // السبونسور غير موجود
            }

            // تحديث التفاصيل
            sponsor.Name = dto.Name ?? sponsor.Name;
            sponsor.Description = dto.Description ?? sponsor.Description;
            sponsor.Website = dto.Website ?? sponsor.Website;

            // إذا كان هناك صورة جديدة تم رفعها، نحدثها
            if (dto.Logo != null)
            {
                sponsor.Logo = FileHelper.UplodeFile(dto.Logo, "Images");
            }

            // حفظ التغييرات
            await dbContext.SaveChangesAsync();
            return true; // تم التعديل بنجاح
        }
        public async Task<bool> RemoveSponsorFromCompetitionAsync(int sponsorId, int competitionId)
        {
            // البحث عن العلاقة بين السبونسور والمسابقات في جدول الوسيط
            var sponsorCompetition = await dbContext.SponsorComptiitions
                                                    .FirstOrDefaultAsync(sc => sc.SponsorID == sponsorId && sc.CompetitionID == competitionId);

            if (sponsorCompetition == null)
            {
                return false; // إذا كانت العلاقة غير موجودة بين السبونسور والمسابقات
            }

            // حذف العلاقة بين السبونسور والمسابقات
            dbContext.SponsorComptiitions.Remove(sponsorCompetition);

            // حفظ التغييرات
            await dbContext.SaveChangesAsync();
            return true; // تم حذف العلاقة بنجاح
        }

        public async Task<List<SponsorDto>> GetSponsorsByNameAsync(string name)
        {
            var sponsors = await dbContext.Sponsors
                .Where(s => s.Name.Contains(name))  // البحث عن السبونسرات التي تحتوي على الاسم المدخل
                .Select(s => new SponsorDto
                {
                    SponsorId = s.SponsorID,
                    Name = s.Name,
                })
                .ToListAsync();

            return sponsors;
        }

    }
}
