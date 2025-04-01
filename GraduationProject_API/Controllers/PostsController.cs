using GraduationProject_Core.Dtos.Post;
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
	public class PostsController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IWebHostEnvironment env;

		public PostsController(IUnitOfWork unitOfWork,IWebHostEnvironment env)
		{
			this.unitOfWork = unitOfWork;
			this.env = env;
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
			
			
			var result = await unitOfWork.iPostRepositry.AddPost(userId.Value, dto,token);
			if (result == "Post added successfully.")
			{
				return Ok("Post added successfully");
			}
			return BadRequest(result);
		}

		[HttpGet("Get-Posts")]
		public async Task<IActionResult> Get(int pageIndex = 1,int PageSize= 5, Post.PostType type=Post.PostType.Global)
		{
			string baseUrl = $"{Request.Scheme}://{Request.Host}";
			var posts=await unitOfWork.iPostRepositry.GetPostDtos(pageIndex, PageSize,type);

			foreach (var item in posts.Posts)
			{
				string relativePath = item.PostImage.Replace(env.WebRootPath, "").Replace("\\", "/");
				item.PostImage = $"{baseUrl}{relativePath}";
				string relativePath1 = item.ProfileImage.Replace(env.WebRootPath, "").Replace("\\", "/");
				item.ProfileImage = $"{baseUrl}{relativePath1}";
			}

			if(posts != null)
			{
				return Ok(posts);
			}
			return BadRequest(posts);
		}
		[HttpGet("Get-Post-Link/{postId}")]
		public async Task<IActionResult> GetPostById(int postId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
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

			var result = await unitOfWork.iPostRepositry.GetPostLink(postId);

			if (result == null)
				return NotFound("Post not found.");

			return Ok(result);
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
