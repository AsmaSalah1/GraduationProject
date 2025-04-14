using GraduationProject_Core.Dtos.Comment;
using GraduationProject_Core.Dtos.Likes;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Infrastructure.Repositories
{
	public class LikeRepository : ILikeRepository
	{
		private readonly ApplicationDbContext dbContext;

		public LikeRepository(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<string> ToggleLikeAsync(int postId, int userId)
		{
			var like = await dbContext.Likes
				.FirstOrDefaultAsync(x => x.PostId == postId && x.UserId == userId);

			if (like != null)
			{
				// إذا كان المستخدم قد وضع إعجابًا مسبقًا، نقوم بإزالته
				dbContext.Likes.Remove(like);
				await dbContext.SaveChangesAsync();
				return "Like Removed Successfully"; // الإعجاب تم إزالته
			}
			else
			{
				// إذا لم يكن قد وضع إعجابًا، نقوم بإضافته
				var newLike = new Like { PostId = postId, UserId = userId };
				dbContext.Likes.Add(newLike);
				await dbContext.SaveChangesAsync();
				return "Like Added Successfully"; // الإعجاب تم إضافته
			}
			
		}
		public async Task<IEnumerable<GetLikesDto>> GetLikesByPostId(int postId)
		{
			var postExists = await dbContext.Posts.AnyAsync(p => p.PostId == postId);
			if (!postExists)
			{
				return Enumerable.Empty<GetLikesDto>();
			}
			return await dbContext.Likes
				.Include(u => u.User)
				.Where(p => p.PostId == postId)
				.Select(x => new GetLikesDto
				{
					LikeId = x.Id,
					UserId=x.UserId,
					UserName = x.User.UserName,
				}).ToListAsync();

		}
	}
}
