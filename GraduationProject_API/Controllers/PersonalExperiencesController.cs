using GraduationProject_Core.Dtos.PersonalExperiance;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PersonalExperiencesController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;

		public PersonalExperiencesController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		[HttpGet("Get-All-Personal-Experiences")]
		public async Task<IActionResult> personalExperiance(int PageIndex = 1, int PageSize = 5)
		{
			var personalExperiance = await unitOfWork.personalExperianceRepositry.GetAllPersonalExperience(PageIndex, PageSize);
			if (personalExperiance == null)
			{
				return NotFound("There is no Personal Experiences ");
			}
			return Ok(personalExperiance);
		}
		[Authorize]
		[HttpPost("Create-your-Personal-Experience")]
		public async Task<IActionResult> CreatePersonalExperience(AddPersonalExperianceDtos dto)
		{
			if (!ModelState.IsValid) { 
			return BadRequest(ModelState);
			}
		
		var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			//التوكن عباة عن سترينغ
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}
			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
            }

           var result = await unitOfWork.personalExperianceRepositry.CreatePersonalExperianceAsync(userId.Value, dto);
           if (result == "PersonalExperience Added Successfully")
           {
         	return Ok("PersonalExperience added successfully");
           }
          return BadRequest(result);
		}
		[Authorize]
		[HttpPut("Update-your-Personal-Experience/{personalExperienceId}")]
		public async Task<IActionResult> UpdatePersonalExperiance(int personalExperienceId ,UpdatePersonalExperianceDtos dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			//التوكن عباة عن سترينغ
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}
			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
			}

			var result = await unitOfWork.personalExperianceRepositry.UpdatePersonalExperianceAsync(userId.Value, personalExperienceId, dto);
			if (result == "Personal Experience updated successfully You must wait Admin to accept it")
			{
				return Ok("Personal Experience updated successfully You must wait Admin to accept it");
			}
			return BadRequest(result);
		}
		[Authorize]
		[HttpDelete("Delete-Experience/{personalExperienceId}")]
		public async Task<IActionResult> DeletePersonalExperience(int personalExperienceId)
		{
			if (personalExperienceId == null)
			{
				return BadRequest("Personal Experience not exist");
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			//التوكن عباة عن سترينغ
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}
			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
			}

			var result = await unitOfWork.personalExperianceRepositry.DeleteExperience(userId.Value,personalExperienceId);
			//if (result == "Experience Deleted Successfully")
			//{
			//	return Ok("Experience Deleted Successfully");
			//}
			//return BadRequest(result);
			return result == "Experience Deleted Successfully"
		    ? Ok(result)
		    : BadRequest(result);
		}

	}
}
