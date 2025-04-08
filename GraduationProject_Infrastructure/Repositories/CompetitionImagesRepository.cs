using GraduationProject_Core.Dtos.Competition;
using GraduationProject_Core.Dtos.CompetitionImage;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Infrastructure.Repositories
{
    public class CompetitionImagesRepository: ICompetitionImagesRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CompetitionImagesRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> AddCompetitionImagesAsync(int CompetitionId, AddCompetitionImageDto dto)
        {
            var competition = await dbContext.Competitions.FindAsync(CompetitionId);
            if (competition == null)
                return false; // المسابقة غير موجودة

            // رفع كل الصور باستخدام دالة "UplodeFile"
            foreach (var image in dto.Images)
            {
                // استخدام دالة رفع الصورة
                var imageName = FileHelper.UplodeFile(image, "Images"); // رفع الصورة
                if (string.IsNullOrEmpty(imageName))
                    continue; // إذا كانت الصورة غير قابلة للرفع، نتجاوزها

                // إنشاء كائن جديد من CompetitionImages وتخزين البيانات في قاعدة البيانات
                var competitionImage = new CompetitionImages
                {
                    CompetitionId = CompetitionId,
                    ImageName = imageName
                };

                await dbContext.CompetitionImages.AddAsync(competitionImage);
            }

            // حفظ التغييرات في قاعدة البيانات
            await dbContext.SaveChangesAsync();
            return true;
        }
     
        public async Task<List<GetImagesByCompetitionDto>> GetImagesByCompetitionIdAsync(int competitionId)
        {
            var images = await dbContext.CompetitionImages
                                        .Where(c => c.CompetitionId == competitionId)
                                        .Select(c => new GetImagesByCompetitionDto
                                        {
                                            CompetitionImagesId = c.CompetitionImagesId, // إرجاع الـ ID
                                            ImageName = c.ImageName
                                        })
                                        .ToListAsync();

            return images;
        }


        // حذف صورة بناءً على معرف الصورة
        public async Task<bool> DeleteCompetitionImageAsync(int imageId)
        {
            var image = await dbContext.CompetitionImages.FindAsync(imageId);
            if (image == null)
                return false;

            dbContext.CompetitionImages.Remove(image);
            await dbContext.SaveChangesAsync();
            return true;
        }

        // تحديث (إضافة صورة جديدة) بدلاً من الصورة القديمة
      
    }
}
