using GraduationProject_Core.Dtos.Competition;
using GraduationProject_Core.Dtos.CompetitionImage;
using GraduationProject_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraduationProject_Core.Models.Competition;
using static GraduationProject_Core.Models.CompetitionImages;
namespace GraduationProject_Core.Interfaces
{
	public interface ICompetitionRepository
	{
		Task<bool> AddCompetitionAsync(CreateCompetitionDto dto, int userId);
		Task<List<Competition>> GetAllCompetitionsAsync();
		Task<List<Competition>> GetCompetitionsByTypeAsync(CompetitionType type);
		Task<bool> DeleteCompetitionAsync(int competitionId);
		Task<bool> UpdateCompetitionAsync(int competitionId, UpdateCompetitionDto dto);
		Task<Competition> GetCompetitionByNameAsync(string competitionName);
		Task<Competition> GetCompetitionByIdAsync(int competitionId);
		Task<bool> UpdateDescriptionAsync(int competitionId, string description);
		Task<bool> UpdateLocationAsync(int competitionId, List<string> locations);
		Task<Competition> GetLastCompetitionByUniversityIdAsync(int universityId);
		Task<List<Competition>> GetCompetitionsByUniversityIdAsync(int universityId);
		Task<bool> AddCompetitionToUniversityAsync(CreateCompetitionDto dto, int universityId, int userId);
		Task<bool> UpdateImage2Async(int competitionId, IFormFile image2);
		//Task<List<CompetitionImages>> GetCompetitionImagesAsync(int competitionId);
		// Task AddCompetitionImagesAsync(int competitionId, List<string> imageNames);
	}
}
