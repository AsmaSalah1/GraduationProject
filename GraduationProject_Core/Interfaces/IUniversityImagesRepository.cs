using GraduationProject_Core.Dtos.UniversityImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
    public interface IUniversityImagesRepository
    {
        Task<bool> AddUniversityImagesAsync(int universityId, AddUniversityImageDto dto);
        Task<List<GetImagesByUniversityDto>> GetImagesByUniversityIdAsync(int universityId);
        Task<bool> DeleteUniversityImageAsync(int imageId);
    }
}
