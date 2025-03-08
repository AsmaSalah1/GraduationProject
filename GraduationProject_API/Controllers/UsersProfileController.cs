using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UsersProfileController : ControllerBase
	{
		private readonly IUserProfileRepositry userProfileRepositry;

		public UsersProfileController(IUserProfileRepositry userProfileRepositry)
		{
			this.userProfileRepositry = userProfileRepositry;
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
				var result = await userProfileRepositry.GetUserProfileAsync(userId.Value);
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

	}
}
