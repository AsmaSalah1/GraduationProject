using GraduationProject_Core.Dtos.PersonalExperiance;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Infrastructure.Repositories
{
	public class PersonalExperianceRepositry : IPersonalExperianceRepositry
	{
		private readonly ApplicationDbContext dbContext;

		public PersonalExperianceRepositry(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<string> CreatePersonalExperianceAsync(int userId, AddPersonalExperianceDtos dto)
		{
			if (dto == null)
			{
				return "Entered data dose not Invalid";
			}

			var P = new PersonalExperience()
			{
				Content = dto.Content,
				IsReviewed=true,
				UserId = userId,
				IsAccepted=false,
				IsDeleted=false,
				DateTime=DateTime.Now
			};
			var result = await dbContext.PersonalExperiences.AddAsync(P);
			await dbContext.SaveChangesAsync();
			if (result != null)
			{

				return "Personal Experience Added Successfully";
			}
			return (result.ToString());
		}

		public async Task<PersonalExperiencePagedResponseDto<GetPersonalExperienceDto>> GetAllPersonalExperience( int PageIndex, int PageSize)
		{
			var personalExperiances = dbContext
									 .PersonalExperiences
									 .Include(p => p.User)
									 .Where(w => w.IsAccepted == true)
									 .Select(x => new GetPersonalExperienceDto
									 {
										 Content = x.Content,
										 DateTime = x.DateTime,
										 ImageName = x.User.Image,
										 UserName = x.User.UserName
									 }).AsQueryable();
			var result = await PaginationAsync(personalExperiances, PageIndex, PageSize);

			return result;		
		}
		public async Task<PersonalExperiencePagedResponseDto<GetPersonalExperienceDto>> PaginationAsync(IQueryable<GetPersonalExperienceDto> query, int PageIndex, int PageSize)
		{
			var allPersonalExperienc = query.Count();
			var personalExperiances = await query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
			var result = new PersonalExperiencePagedResponseDto<GetPersonalExperienceDto>()
			{
				PersonalExperiances= personalExperiances,
				TotalPersonalExperienc= allPersonalExperienc,
				PageSize = PageSize,
				PageIndex = PageIndex
			};
			return result;
		}
		public async Task<string> UpdatePersonalExperianceAsync(int userId, int experienceId, UpdatePersonalExperianceDtos updateDto)
		{
			if (updateDto == null || string.IsNullOrWhiteSpace(updateDto.Content))
			{
				return "Invalid update data";
			}
			var experience = await dbContext.PersonalExperiences
				.SingleOrDefaultAsync(x => x.UserId == userId && x.PersonalExperienceId == experienceId);
			if (experience == null)
			{
				return "Personal Experience not found";
			}
			// تحديث العنوان إذا كان موجودًا
			if (!string.IsNullOrEmpty(updateDto.Content))
			{
				experience.Content = updateDto.Content;
				experience.IsAccepted = false;
				experience.IsDeleted=false;
				experience.IsReviewed = true;
			}
			
			 dbContext.PersonalExperiences.Update(experience);
			await dbContext.SaveChangesAsync();
			return "Personal Experience updated successfully You must wait Admin to accept it";

		}
		public async Task<string> DeleteExperience(int userId, int experienceId)
		{
			//if(experienceId == null || userId == null)
			//{
			//	return "User or Experience not found";
			//}
			var experience = await dbContext.PersonalExperiences
				.FirstOrDefaultAsync(x=>x.PersonalExperienceId==experienceId && x.UserId==userId);
			if(experience == null)
			{
				return "User or Experience not found";

			}
			dbContext.PersonalExperiences.Remove(experience);
			await dbContext.SaveChangesAsync();
			return "Experience Deleted Successfully";
		}

	}
}
