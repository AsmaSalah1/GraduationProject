using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class Post
	{
		[Key]
		public int PostId { get; set; }

		[ForeignKey(nameof(User))]
		public int Userld { get; set; }

		public string Description { get; set; }
		public string Title { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Image { get; set; }
	//	public string Url { get; set; }

		// علاقة مع جدول المستخدم (User) - المنشور كتبه مستخدم واحد
		public User User { get; set; }
	}
}
