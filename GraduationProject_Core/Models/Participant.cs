using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Models
{
	public class Participant
	{
		[Key]
		public int ParticipantId { get; set; }
		public string ParticipantName { get; set; }
		public ICollection<TeamParticipant> TeamsParticipant { get; set; }=new HashSet<TeamParticipant>();
	}
}
