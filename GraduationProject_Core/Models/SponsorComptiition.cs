using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class SponsorComptiition
	{

		public int SponsorID { get; set; }
		public int CompetitionID { get; set; }
		public Sponsor Sponsor { get; set; }
		public Competition Competition { get; set; }
	}
}
