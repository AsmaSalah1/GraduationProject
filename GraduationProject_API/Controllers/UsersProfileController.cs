using GraduationProject_Core.Dtos.UserProfile;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
	//[Authorize]
	[Route("[controller]")]
	[ApiController]
	public class UsersProfileController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IWebHostEnvironment env;

		//private readonly IUserProfileRepositry userProfileRepositry;

		public UsersProfileController(IUnitOfWork unitOfWork,IWebHostEnvironment env )
		{
			this.unitOfWork = unitOfWork;
			this.env = env;
			//	this.userProfileRepositry = userProfileRepositry;
		}

		[HttpGet("Profile/{UserId}")]
		public async Task<IActionResult> GetUserProfile(int UserId)
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
				string baseUrl = $"{Request.Scheme}://{Request.Host}";
				var result = await unitOfWork.userProfileRepositry.GetUserProfileAsync(UserId);
				//if (!string.IsNullOrEmpty(result.Image) && result.Image.Length > env.WebRootPath.Length)
				//{
				//	result.Image = $"{baseUrl}{result.Image.Substring(env.WebRootPath.Length).Replace("\\", "/")}";
				//}
				if (!string.IsNullOrEmpty(result.Image) && !result.Image.StartsWith("http", StringComparison.OrdinalIgnoreCase))
				{
					result.Image = $"{baseUrl}{result.Image.Replace("\\", "/").Replace(env.WebRootPath, "")}";
				}
				//		result.Image=$"{baseUrl}{result.Image.Substring(env.WebRootPath.Length).Replace("\\","/")}";
				if (!string.IsNullOrEmpty(result.Cv) && !result.Cv.StartsWith("http", StringComparison.OrdinalIgnoreCase))
				{
					result.Cv = $"{baseUrl}{result.Cv.Replace("\\", "/").Replace(env.WebRootPath, "")}";
				}


				//result.Cv= $"{baseUrl}{result.Cv.Substring(env.WebRootPath.Length).Replace("\\", "/")}";
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

		[HttpGet("Get-User-Profile-By-Name/{userName}")]
		public async Task<IActionResult> GetUserProfile(string userName)
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
			var users = await unitOfWork.userProfileRepositry.GetUsersByNameAsync(userName);
			if (users == null || users.Count == 0)
			{
				return NotFound("No users found with the given name");
			}
			return Ok(users);
		}
		//[HttpGet("Profile/{userId_profile}")]
		//public async Task<IActionResult> GetUserProfileById(int userId_profile)
		//{
		//	var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
		//	//التوكن عباة عن سترينغ
		//	if (string.IsNullOrEmpty(token))
		//	{
		//		return Unauthorized("Token is missing");
		//	}
		//	var userId = ExtractClaims.ExtractUserId(token);
		//	if (!userId.HasValue)
		//	{
		//		return Unauthorized("Invalid user token");
		//	}

		//	try
		//	{
		//		string baseUrl = $"{Request.Scheme}://{Request.Host}";
		//		var result = await unitOfWork.userProfileRepositry.GetUserProfileAsync(userId.Value);
		//		//if (!string.IsNullOrEmpty(result.Image) && result.Image.Length > env.WebRootPath.Length)
		//		//{
		//		//	result.Image = $"{baseUrl}{result.Image.Substring(env.WebRootPath.Length).Replace("\\", "/")}";
		//		//}
		//		if (!string.IsNullOrEmpty(result.Image) && !result.Image.StartsWith("http", StringComparison.OrdinalIgnoreCase))
		//		{
		//			result.Image = $"{baseUrl}{result.Image.Replace("\\", "/").Replace(env.WebRootPath, "")}";
		//		}
		//		//		result.Image=$"{baseUrl}{result.Image.Substring(env.WebRootPath.Length).Replace("\\","/")}";
		//		if (!string.IsNullOrEmpty(result.Cv) && !result.Cv.StartsWith("http", StringComparison.OrdinalIgnoreCase))
		//		{
		//			result.Cv = $"{baseUrl}{result.Cv.Replace("\\", "/").Replace(env.WebRootPath, "")}";
		//		}

		//		return Ok(result);
		//	}
		//	catch (KeyNotFoundException ex)
		//	{
		//		return NotFound(new { message = ex.Message });
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
		//	}


		//}

	}
}
