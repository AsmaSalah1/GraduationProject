using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class Comment
	{
		public int Id { get; set; }
		[ForeignKey(nameof(Post))]
		public int PostId { get; set; }

		[ForeignKey(nameof(User))]
		public int UserId { get; set; }
		//قيمة افتراضية انه empty
		public string Content { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		// العلاقة مع المنشور
		public Post? Post { get; set; }
		public User User { get; set; }
	}
}
