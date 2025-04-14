using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Comment
{
	public class GetCommentDto
	{
		public int CommentId { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserImage { get; set; }
		public string CommentContent { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
