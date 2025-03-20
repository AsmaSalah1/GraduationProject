using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class UniversityImages
	{
		[Key]
		public int UniversityImagesId { get; set; }
		[ForeignKey(nameof(University))]
		public int UniversityId { get; set; }
		public string ImageName { get; set; }
		public University University { get; set; }

	}
}
