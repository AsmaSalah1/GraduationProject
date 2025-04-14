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
		private readonly IWebHostEnvironment env;

		public PersonalExperiencesController(IUnitOfWork unitOfWork , IWebHostEnvironment env)
		{
			this.unitOfWork = unitOfWork;
			this.env = env;
		}
		[HttpGet("Get-All-Personal-Experiences")]
		public async Task<IActionResult> personalExperiance(int PageIndex = 1, int PageSize = 5)
		{
			string baseUrl = $"{Request.Scheme}://{Request.Host}";
			var personalExperiance = await unitOfWork.personalExperianceRepositry.GetAllPersonalExperience(PageIndex, PageSize);
			//personalExperiance.PersonalExperiances = personalExperiance.PersonalExperiances.ToList();

			
			foreach (var item in personalExperiance.PersonalExperiances)
			{
				string relativePath = item.ImageName.Replace(env.WebRootPath, "").Replace("\\", "/");
				item.ImageName = $"{baseUrl}{relativePath}";
			}

			if (personalExperiance == null)
			{
				return NotFound("There is no Personal Experiences ");
			}
			return Ok(personalExperiance);
		}

		[Authorize(Roles = "User")]
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
           if (result == "Personal Experience Added Successfully")
           {
         	return Ok("Personal Experience Added Successfully");
           }
          return BadRequest(result);
		}
		
		[Authorize(Roles = "User")]
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

		[Authorize(Roles = "User")]
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
		
		[Authorize(Roles = "Admin")]
		[HttpGet("Get-UnReviewed-Personal-Experience")]
		public async Task<IActionResult> GetUnReviewedPersonalExperience(int PageIndex = 1, int PageSize = 5)
		{

			string baseUrl = $"{Request.Scheme}://{Request.Host}";
			var personalExperiance = await unitOfWork.personalExperianceRepositry.GetUnReviewedPersonalExperience(PageIndex, PageSize);

			foreach (var item in personalExperiance.PersonalExperiances)
			{
				string relativePath = item.ImageName.Replace(env.WebRootPath, "").Replace("\\", "/");
				item.ImageName = $"{baseUrl}{relativePath}";
			}
			if (personalExperiance == null)
			{
				return NotFound("There is no UnReviewed Personal Experiences ");
			}
			return Ok(personalExperiance);
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("Accept-Personal-Experiences-By-Admin/{personalExperienceId}")]
		public async Task<IActionResult> AcceptPersonalExperienceByAdmin(int personalExperienceId)
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
			var userRole = ExtractClaims.ExtractRoles(token);
			if (userRole[0] != "Admin")
			{
				return Unauthorized("Invalid user token");
			}

			var result = await unitOfWork.personalExperianceRepositry.AcceptPersonalExperience(personalExperienceId);
			
			return result == "Experience Accepted Successfully"
			? Ok(result)
			: BadRequest(result);
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("Delete-Personal-Experience-By-Admin/{personalExperienceId}")]
		public async Task<IActionResult> DeletePersonalExperienceByAdmin(int personalExperienceId)
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
			var userRole = ExtractClaims.ExtractRoles(token);
			if (userRole[0] != "Admin")
			{
				return Unauthorized("Invalid user token");
			}

			var result = await unitOfWork.personalExperianceRepositry.DeletePersonalExperienceByAdmin(personalExperienceId);
			return result == "Experience Deleted Successfully"
			? Ok(result)
			: BadRequest(result);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("Get-UnReviewed-Personal-Experience-To-Reviewed")]
		public async Task<IActionResult> Get_UnReviewed_Personal_Experience()
		{

			string baseUrl = $"{Request.Scheme}://{Request.Host}";
			var personalExperiance = await unitOfWork.personalExperianceRepositry.Get_UnReviewed_Personal_Experience();

			foreach (var item in personalExperiance)
			{
				string relativePath = item.ImageName.Replace(env.WebRootPath, "").Replace("\\", "/");
				item.ImageName = $"{baseUrl}{relativePath}";
			}
			if (personalExperiance == null)
			{
				return NotFound("There is no UnReviewed Personal Experiences ");
			}
			return Ok(personalExperiance);
		}
	}
}
