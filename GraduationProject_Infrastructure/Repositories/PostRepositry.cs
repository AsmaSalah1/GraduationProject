using GraduationProject_Core.Dtos.Post;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Infrastructure.Repositories
{
	public class PostRepositry: IPostRepositry
	{
		private readonly ApplicationDbContext dbContext;

		public PostRepositry(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<string> AddPost(int userId, CreatePostDto dto,string token)
		{
			var role = ExtractClaims.ExtractRoles(token);
			if (role == null)
			{//اليوزر ما اله صلاحية
				return "The user does not have permission.";
			}
			var post = new Post()
			{
				CreatedAt = DateTime.Now,
				Description = dto.Description,
				Title = dto.Title,
				UserId = userId,
			};

			// إذا كانت الصورة موجودة
			if (dto.Image != null)
			{
				string newFileName = FileHelper.UplodeFile(dto.Image, "Images");
				post.Image = newFileName;
			}
			if (role[0] == "Admin")
			{
				post.Posttype = Post.PostType.Global;
			}
			if (role[0] == "SuperAdmin")
			{
				post.Posttype = Post.PostType.Local;
			}
			// إضافة الـ Post إلى قاعدة البيانات
			var result = await dbContext.Posts.AddAsync(post);

			// حفظ التغييرات
			var saved = await dbContext.SaveChangesAsync();

			// التحقق إذا تم حفظ الـ Post بنجاح
			if (saved > 0)
			{
				return "Post added successfully.";
			}
			else
			{
				return "Failed to add the post.";
			}
		}

		public async Task<PostPagedResponseDto<GetPostDto>> GetPostDtos(int PageIndex, int PageSize, Post.PostType type)
		{
			var posts = dbContext.Posts
				.Include(u => u.User).ThenInclude(w=>w.University)
				.Include(c => c.Comments)
				.Where(q=>q.Posttype==type)
				.Select(x => new GetPostDto
				{
					PostId=x.PostId,
					Description = x.Description,
					PosterName = x.User.UserName,
					PostImage = x.Image,
					ProfileImage = x.User.Image,
					Title = x.Title,
					university = x.User.University.Name,
					TotalLikes=x.Likes.Count(),
					TotalComments = x.Comments.Count()
				}).AsNoTracking()
				.AsQueryable();	

			if(posts == null)
			{
				return null;
			}
		
			var result=await PaginationAsync(posts, PageIndex, PageSize);
			return result;
		}

		public async Task<PostPagedResponseDto<GetPostDto>> PaginationAsync(IQueryable<GetPostDto> query, int PageIndex, int PageSize)
		{
			var allPostes = query.Count();
			var posts = await query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
			var result = new PostPagedResponseDto<GetPostDto>()
			{
				PageIndex = PageIndex,
				PageSize = PageSize,
				Posts = posts,
				TotalPosts = allPostes
			};
			return result;
		}

		public async Task<string> DeletePost(int userId, int PostId)
		{
			var post = await dbContext
				.Posts
				.FirstOrDefaultAsync(x => x.UserId == userId && x.PostId == PostId);
				
			if (post == null)
			{
				return "User or post not found";

			}
			FileHelper.DeleteFile(post.Image, "Images");
			dbContext.Posts.Remove(post);
			await dbContext.SaveChangesAsync();

			return "post Deleted Successfully";
		}

		//public async Task<string> GetPostLink(int postId)
		//{
		//	var post = await dbContext.Posts.FindAsync(postId);
		//	if (post == null) {
		//		return "Post not exist";
		//	}
		//	return $"https://localhost:7024/Posts/Get-Post-Link/{postId} ";
		//}
		public async Task<GetPostDto> GetPostLink(int postId)
		{
			var post = await dbContext.Posts.Include(x=>x.User).FirstOrDefaultAsync(c=>c.PostId==postId);
			if (post == null)
			{
				return null;
			}

			var postdto=new GetPostDto()
			{
				PostId = post.PostId,
				Description=post.Description,
				PosterName=post.User.UserName,
				PostImage=post.Image,
				ProfileImage=post.User.Image,
				Title=post.Title,
				university=post.User.UserName
			};
			return postdto;
		}
	}
}
