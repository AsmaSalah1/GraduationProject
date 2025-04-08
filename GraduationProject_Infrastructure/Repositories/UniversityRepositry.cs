using GraduationProject_Core.Dtos.University;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Infrastructure.Repositories
{
    public class UniversityRepositry : IUniversityRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public UniversityRepositry(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // دالة إضافة جامعة جديدة
        public async Task<bool> AddUniversityAsync(CreateUniversityDto dto)
        {
            string newLogoName = null;
            string newImageName = null;

            // تحقق إذا كانت هناك صورة جديدة تم رفعها
            if (dto.Logo != null)
            {
                newLogoName = FileHelper.UplodeFile(dto.Logo, "Images");
            }

            // تحقق إذا كانت هناك صورة جديدة تم رفعها للـ ImageName
            if (dto.ImageName != null)
            {
                newImageName = FileHelper.UplodeFile(dto.ImageName, "Images");
            }

            // إنشاء كائن الجامعة (University)
            var university = new University
            {
                Name = dto.Name,
                Logo = newLogoName,
                Description = dto.Description,
                Location = dto.Location,
                Url = dto.Url,
                PhoneNumber = dto.PhoneNumber,
                Gmail = dto.Gmail,
                ImageName = newImageName
            };

            // إضافة الجامعة إلى قاعدة البيانات
            await dbContext.Universities.AddAsync(university);
            await dbContext.SaveChangesAsync();

            return true;
        }
        // دالة لجلب جميع الجامعات
        public async Task<List<University>> GetAllUniversitiesAsync()
        {
            return await dbContext.Universities
                .Select(u => new University
                {
                    UniversityId = u.UniversityId,
                    Name = u.Name,
                    ImageName = u.ImageName  // صورة الجامعة
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateUniversityAsync(int universityId, UpdateUniversityDto dto)
        {
            var university = await dbContext.Universities.FindAsync(universityId);

            if (university == null)
            {
                return false;  // الجامعة غير موجودة
            }

            // تحديث البيانات بناءً على الـ DTO
            university.Name = dto.Name ?? university.Name;
            university.Description = dto.Description ?? university.Description;
            university.Location = dto.Location ?? university.Location;
            university.Url = dto.Url ?? university.Url;
            university.PhoneNumber = dto.PhoneNumber ?? university.PhoneNumber;
            university.Gmail = dto.Gmail ?? university.Gmail;

            // إذا تم رفع صورة جديدة
            if (dto.Logo != null)
            {
                university.Logo = FileHelper.UplodeFile(dto.Logo, "Images");
            }

            if (dto.ImageName != null)
            {
                university.ImageName = FileHelper.UplodeFile(dto.ImageName, "Images");
            }

            // حفظ التعديلات في قاعدة البيانات
            dbContext.Universities.Update(university);
            await dbContext.SaveChangesAsync();

            return true;  // تم التحديث بنجاح
        }

        public async Task<bool> DeleteUniversityAsync(int universityId)
        {
            // البحث عن الجامعة باستخدام الـ universityId
            var university = await dbContext.Universities.FindAsync(universityId);

            if (university == null)
            {
                return false;  // الجامعة غير موجودة
            }

            // حذف الجامعة من قاعدة البيانات
            dbContext.Universities.Remove(university);
            await dbContext.SaveChangesAsync();

            return true;  // تم الحذف بنجاح
        }

        public async Task<University> GetUniversityByIdAsync(int universityId)
        {
            // البحث عن الجامعة باستخدام الـ universityId
            var university = await dbContext.Universities
                .Where(u => u.UniversityId == universityId)
                .FirstOrDefaultAsync();

            return university;
        }

    }
}
