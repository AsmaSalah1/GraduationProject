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
	}
}
