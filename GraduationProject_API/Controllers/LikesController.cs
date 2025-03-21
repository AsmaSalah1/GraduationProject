using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class LikesController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;

		public LikesController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		// دالة API لتبديل حالة الإعجاب (إضافة أو إزالة الإعجاب)
		[HttpPost("toggle/{postId}")]
		public async Task<IActionResult> ToggleLike(int postId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			// التحقق من وجود التوكن
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized(new { message = "Token is missing" });
			}
			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
			}

			var result = await unitOfWork.iLikeRepository.ToggleLikeAsync(postId, userId.Value);

			if (result == "Like Added Successfully")
			{
				return Ok(new { Message = "تم إضافة الإعجاب!" });
			}
			else if (result == "Like Removed Successfully")
			{
				return Ok(new { Message = "تم إزالة الإعجاب!" });
			}
			else
			{
				return BadRequest(new { Message = "حدث خطأ ما أثناء العملية!" });
			}
		}

	
	}
}
