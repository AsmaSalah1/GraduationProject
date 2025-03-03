using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class CompetitionImages
	{
		[Key]
		public int CompetitionImagesId { get; set; }
		[ForeignKey(nameof(University))]
		public int CompetitionId { get; set; }
		public string ImageName { get; set; }
		public Competition Competition { get; set; }

	}
}
