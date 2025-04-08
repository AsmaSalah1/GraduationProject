using GraduationProject_Core.Dtos.Post;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
	[Authorize(Roles = "Admin,SuperAdmin")]

		[HttpPost("Create-post")]
		public async Task<IActionResult> CreatePost(CreatePostDto dto)
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
		
				string relativePath1 = item.ProfileImage.Replace(env.WebRootPath, "").Replace("\\", "/");
				item.ProfileImage = $"{baseUrl}{relativePath1}";

				//ImageUrl = $"{baseUrl}/Images/{item.ImageName}"
				// تحويل مسار الصورة إلى URL كامل
				//    string relativePath = item.ImageName.Replace(env.WebRootPath, "").Replace("\\", "/");
				if (item.PostImage != null)
					item.PostImage = $"{baseUrl}/Images/{item.PostImage}";
			}

			if (posts != null)
			{
				return Ok(posts);
			}
			return BadRequest(posts);
		}


		[HttpGet("Get-Posts-By-University-Name")]
		public async Task<IActionResult> GetPostsByUniversityName(
	[FromQuery] int universityId,
	[FromQuery] int pageIndex = 1,
	[FromQuery] int PageSize = 5
	//[FromQuery] Post.PostType type = Post.PostType.Local
	)
		{
			string baseUrl = $"{Request.Scheme}://{Request.Host}";

			var posts = await unitOfWork.iPostRepositry.GetPostByUniversityName(universityId,pageIndex, PageSize, Post.PostType.Local);
			foreach (var item in posts.Posts)
			{

				string relativePath1 = item.ProfileImage.Replace(env.WebRootPath, "").Replace("\\", "/");
				item.ProfileImage = $"{baseUrl}{relativePath1}";

				//ImageUrl = $"{baseUrl}/Images/{item.ImageName}"
				// تحويل مسار الصورة إلى URL كامل
				//    string relativePath = item.ImageName.Replace(env.WebRootPath, "").Replace("\\", "/");
				if(item.PostImage != null) 
				item.PostImage = $"{baseUrl}/Images/{item.PostImage}";
			}

			if (posts != null)
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
	
		
		[Authorize(Roles = "Admin,SuperAdmin")]
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
