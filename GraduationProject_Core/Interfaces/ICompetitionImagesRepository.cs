using GraduationProject_Core.Dtos.Competition;
using GraduationProject_Core.Dtos.CompetitionImage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
    public interface ICompetitionImagesRepository
    {
      //  Task<bool> AddCompetitionImagesAsync(AddCompetitionImageDto dto);
        //Task<List<string>> GetImagesByCompetitionIdAsync(int competitionId);
     //   Task<List<CompetitionImageDto>> GetImagesByCompetitionIdAsync(int competitionId);
        Task<bool> AddCompetitionImagesAsync(int CompetitionId, AddCompetitionImageDto dto);
        Task<List<GetImagesByCompetitionDto>> GetImagesByCompetitionIdAsync(int competitionId);
            Task<bool> DeleteCompetitionImageAsync(int imageId);
        //Task<bool> UpdateCompetitionImageAsync(int imageId, IFormFile newImage);
    }
}
