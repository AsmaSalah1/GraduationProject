using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class PersonalExperience
	{
		[Key]
		public int PersonalExperienceId { get; set; }

		[ForeignKey(nameof(User))]
		public int UserId { get; set; }
		public string Content { get; set; }
		public bool IsReviewed { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsAccepted { get; set; }
		public DateTime DateTime { get; set; }


		// علاقة مع جدول المستخدم (User)
		public User User { get; set; }
	}
}
