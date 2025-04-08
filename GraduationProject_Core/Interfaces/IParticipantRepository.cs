using GraduationProject_Core.Dtos.Participant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
	public interface IParticipantRepository
	{
		Task<bool> AddParticipantToTeamAsync(int teamId, string participantName, DateTime year);
		Task<bool> RemoveParticipantFromTeamAsync(int teamId, int participantId);
		Task<ParticipantDtos> GetParticipantByNameAsync(string participantName);
		Task<List<ParticipantDtos>> GetAllParticipantsAsync();
		//Task<bool> AddParticipantToTeamAsync(int teamId, int participantId, DateTime year);

		Task<bool> AddoldParticipantToTeamAsync(int teamId, int participantId, DateTime year);
	}
}
