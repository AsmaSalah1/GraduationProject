using GraduationProject_Core.Dtos.Participant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Team
{
	public class CreateTeamDto
	{
		[Required]
		public string TeamName { get; set; }

		[Required]
		public string UniversityName { get; set; }

		public string? Coach { get; set; }

		[Required]
		[MinLength(1)]
		[MaxLength(3)]
		public List<AddParticipantDto> Participants { get; set; }// = new List<ParticipantDto>(); // تفادي NullReferenceException

		[Required]
		public DateTime Year { get; set; }

	}
}
