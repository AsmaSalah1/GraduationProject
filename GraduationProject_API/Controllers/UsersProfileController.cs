using GraduationProject_Core.Dtos.UserProfile;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
	[Authorize]
	[Route("[controller]")]
	[ApiController]
	public class UsersProfileController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;

		//private readonly IUserProfileRepositry userProfileRepositry;

		public UsersProfileController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
			//	this.userProfileRepositry = userProfileRepositry;
		}

		[HttpGet("Profile")]
		public async Task<IActionResult> GetUserProfile()
		{
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
				
			try
			{
				var result = await unitOfWork.userProfileRepositry.GetUserProfileAsync(userId.Value);
				return Ok(result);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
			}

			
		}

		[HttpPut("Update-Profile")]
		public async Task<IActionResult> UpdateProfile([FromForm]UpdateProfileDtos dto)
		{
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
			var result = await unitOfWork.userProfileRepositry.UpdateUserProfileAsync(userId.Value, dto);
			if (result)
			{
			return Ok("User Information Updated Successfully");
			}
			return BadRequest(result);
		}
	}
}
