using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class QAA
	{
		[Key]
		public int QAAID { get; set; }

		[ForeignKey(nameof(User))]
		public int UserID { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }

		// علاقة مع جدول المستخدم (User)
		public User User { get; set; }
	}
}
