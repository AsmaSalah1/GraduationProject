using GraduationProject_Core.Dtos.University;
using GraduationProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
    public interface IUniversityRepositry
    {
        Task<bool> AddUniversityAsync(CreateUniversityDto dto);
        Task<List<University>> GetAllUniversitiesAsync();
        Task<bool> UpdateUniversityAsync(int universityId, UpdateUniversityDto dto);
        Task<bool> DeleteUniversityAsync(int universityId);
        Task<University> GetUniversityByIdAsync(int universityId);
    }
}
