using GraduationProject_Core.Dtos.Comment;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CommentsController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;

		public CommentsController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		[HttpPost("Add-Comment/{postId}")]
		public async Task<IActionResult> AddComment(int postId , AddCommentDto dto)
		{
			if (!ModelState.IsValid) { 
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

			var result = await unitOfWork.iCommentRepositry.AddComment(postId, userId.Value, dto);
			if (result == "Comment added successfully.")
			{
				return Ok("Comment added successfully");
			}
			return BadRequest(result);
		}
		
		[HttpGet("Get-comments/{postId}")]
		public async Task<IActionResult> GetComments(int postId)
		{
			// استدعاء الدالة لاسترجاع التعليقات
			var comments = await unitOfWork.iCommentRepositry.GetCommentsByPostId(postId);

			// التحقق من وجود تعليقات
			if (comments == null || !comments.Any())
			{
				return NotFound("No comments found for this post.");
			}

			return Ok(comments);
		}
	
		[HttpDelete("DeleteQAA/{commentId}")]
		public async Task<IActionResult> DeleteQAA(int commentId)
		{
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}

			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
			}

			var result = await unitOfWork.iCommentRepositry.DeleteComment(commentId, userId.Value);
			if (result == "Comment Deleted Successfully")
			{
				return Ok(result);
			}

			return NotFound(result);
		}
	}
}
