using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class TeamParticipant
	{
		public int TeamId { get; set; }
		public int ParticipantId { get; set; }
		public DateTime Year { get; set; }
		public Team Teams { get; set; }
		public Participant Participant { get; set; }
	}
}
