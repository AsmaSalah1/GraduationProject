using GraduationProject_Core.Dtos.PersonalExperiance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
	public interface IPersonalExperianceRepositry
	{
		Task<string> CreatePersonalExperianceAsync(int userId , AddPersonalExperianceDtos dto);
		Task<string> UpdatePersonalExperianceAsync(int userIdd,int experienceId, UpdatePersonalExperianceDtos updateDto);
		Task<PersonalExperiencePagedResponseDto<GetPersonalExperienceDto>> GetAllPersonalExperience(int PageIndex, int PageSize);
		Task<PersonalExperiencePagedResponseDto<GetPersonalExperienceDto>> PaginationAsync(IQueryable<GetPersonalExperienceDto> query, int PageIndex, int PageSize);
		Task<string> DeleteExperience(int userId, int experienceId);
	}
}
