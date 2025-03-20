using GraduationProject_Core.Dtos.Comment;
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
	public class CommentRepositry : ICommentRepositry
	{
		private readonly ApplicationDbContext dbContext;

		public CommentRepositry(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<string> AddComment(int postId, int userId, AddCommentDto dto)
		{
			// التحقق من وجود المنشور
			var post = await dbContext.Posts.FindAsync(postId);
			if (post == null)
			{
				return "Post not found.";
			}

			// إنشاء الكائن Comment
			var comment = new Comment
			{
				Content = dto.Content,
				CreatedAt = DateTime.Now,
				UserId = userId,
				PostId = postId
			};

			// إضافة التعليق إلى قاعدة البيانات
			await dbContext.Comments.AddAsync(comment);
			var saved = await dbContext.SaveChangesAsync();

			// التحقق من نجاح الحفظ
			return saved > 0 ? "Comment added successfully." : "Failed to add comment.";
		}

		public async Task<IEnumerable<GetCommentDto>> GetCommentsByPostId(int postId)
		{
			var postExists = await dbContext.Posts.AnyAsync(p => p.PostId == postId);
			if (!postExists)
			{
				return Enumerable.Empty<GetCommentDto>();
			}
			return await dbContext.Comments
				.Include(u => u.User)
				.Where(p => p.PostId == postId)
				.OrderByDescending(c => c.CreatedAt)
				.Select(x => new GetCommentDto
				{
					UserName=x.User.UserName,
					CreatedAt = x.CreatedAt,
					CommentContent = x.Content,
					UserImage = x.User.Image
				}).ToListAsync();

		}

		public async Task<string> DeleteComment(int CommentId, int userId)
		{
			var comment = await dbContext.Comments.FirstOrDefaultAsync(q => q.Id == CommentId && q.UserId == userId);

			if (comment == null)
			{
				return " Comment not Exist or User Unauthorized"; // لم يتم العثور على السؤال أو المستخدم غير مخوّل
			}

			dbContext.Comments.Remove(comment);
			await dbContext.SaveChangesAsync();
			return "Comment Deleted Successfully";
		}
	}
}
