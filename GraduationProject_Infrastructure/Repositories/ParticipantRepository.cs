using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Infrastructure.Repositories
{
	public class ParticipantRepository:IParticipantRepository
	{
		private readonly ApplicationDbContext dbContext;

		public ParticipantRepository(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
	
		public async Task<bool> AddParticipantToTeamAsync(int teamId, string participantName, DateTime year)
		{
			var team = await dbContext.Teams
				.Include(t => t.TeamsParticipant)
				.FirstOrDefaultAsync(t => t.TeamId == teamId);

			if (team == null)
				return false; // الفريق غير موجود

			// التأكد من أن الفريق لا يحتوي على أكثر من 3 مشاركين
			if (team.TeamsParticipant.Count >= 3)
				return false; // الفريق ممتلئ بالفعل

			// البحث عن المشارك حسب الاسم
			var participant = await dbContext.Participants
				.FirstOrDefaultAsync(p => p.ParticipantName == participantName);

			// إذا لم يكن موجودًا، قم بإنشائه
			if (participant == null)
			{
				participant = new Participant
				{
					ParticipantName = participantName
				};

				await dbContext.Participants.AddAsync(participant);
				await dbContext.SaveChangesAsync();
			}

			// التأكد من أن المشارك غير مضاف مسبقًا إلى الفريق
			if (team.TeamsParticipant.Any(tp => tp.ParticipantId == participant.ParticipantId))
				return false; // المشارك مضاف بالفعل

			var teamParticipant = new TeamParticipant
			{
				TeamId = teamId,
				ParticipantId = participant.ParticipantId,
				Year = year
			};

			dbContext.TeamsParticipants.Add(teamParticipant);
			await dbContext.SaveChangesAsync();
			return true;
		}
		// حذف طالب من فريق
		public async Task<bool> RemoveParticipantFromTeamAsync(int teamId, int participantId)
		{
			var teamParticipant = await dbContext.TeamsParticipants
				.FirstOrDefaultAsync(tp => tp.TeamId == teamId && tp.ParticipantId == participantId);

			if (teamParticipant == null)
				return false; // الطالب غير مضاف إلى الفريق

			dbContext.TeamsParticipants.Remove(teamParticipant);
			await dbContext.SaveChangesAsync();
			return true;
		}

	}
}
