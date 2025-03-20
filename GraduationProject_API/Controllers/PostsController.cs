using GraduationProject_Core.Dtos.Post;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PostsController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;

		public PostsController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		[HttpPost("Create-post")]
		public async Task<IActionResult> CreatePost(CreatePostDto dto) {
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

			var result = await unitOfWork.iPostRepositry.AddPost(userId.Value, dto);
			if (result == "Post added successfully.")
			{
				return Ok("Post added successfully");
			}
			return BadRequest(result);
		}

		[HttpGet("Get-Posts")]
		public async Task<IActionResult> Get(int pageIndex = 1,int PageSize= 5)
		{
			var posts=await unitOfWork.iPostRepositry.GetPostDtos(pageIndex, PageSize);
			if(posts != null)
			{
				return Ok(posts);
			}
			return BadRequest(posts);
		}

		[HttpDelete("Delete-Post/{postId}")]
		public async Task<IActionResult> Delete(int postId)
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

			var result = await unitOfWork.iPostRepositry.DeletePost(userId.Value, postId);
			if (result== "post Deleted Successfully")
			{
				return Ok(result);
			}

			return NotFound("Post not found or unauthorized");
		}
	
	}
}
