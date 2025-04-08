using GraduationProject_Core.Dtos.UniversityImage;
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
    public class UniversityImagesRepository : IUniversityImagesRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UniversityImagesRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // إضافة صور جديدة للجامعة
        public async Task<bool> AddUniversityImagesAsync(int universityId, AddUniversityImageDto dto)
        {
            var university = await dbContext.Universities.FindAsync(universityId);
            if (university == null)
                return false; // الجامعة غير موجودة

            // رفع الصور باستخدام دالة "UplodeFile"
            foreach (var image in dto.Images)
            {
                var imageName = FileHelper.UplodeFile(image, "Images");
                if (string.IsNullOrEmpty(imageName))
                    continue;

                var universityImage = new UniversityImages
                {
                    UniversityId = universityId,
                    ImageName = imageName
                };

                await dbContext.UniversityImages.AddAsync(universityImage);
            }

            await dbContext.SaveChangesAsync();
            return true;
        }

        // الحصول على صور الجامعة
        public async Task<List<GetImagesByUniversityDto>> GetImagesByUniversityIdAsync(int universityId)
        {
            var images = await dbContext.UniversityImages
                                         .Where(u => u.UniversityId == universityId)
                                         .Select(u => new GetImagesByUniversityDto
                                         {
                                             UniversityImagesId = u.UniversityImagesId,
                                             ImageName = u.ImageName
                                         })
                                         .ToListAsync();

            return images;
        }

        // حذف صورة للجامعة
        public async Task<bool> DeleteUniversityImageAsync(int imageId)
        {
            var image = await dbContext.UniversityImages.FindAsync(imageId);
            if (image == null)
                return false;

            dbContext.UniversityImages.Remove(image);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
