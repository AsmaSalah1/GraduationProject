using GraduationProject_Core.Dtos.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
	public interface ICommentRepositry
	{
		Task<string> AddComment(int postId,int userId ,AddCommentDto dto);
		Task<IEnumerable<GetCommentDto>> GetCommentsByPostId(int postId);
		Task<string> DeleteComment (int CommentId , int userId);
	}
}
