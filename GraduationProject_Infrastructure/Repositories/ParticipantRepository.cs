using GraduationProject_Core.Dtos.Participant;
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
	public class ParticipantRepository : IParticipantRepository
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
		public async Task<List<ParticipantDtos>> GetAllParticipantsAsync()
		{
			// استرجاع كل المشاركين مع الـ ParticipantId و الـ ParticipantName
			var participants = await dbContext.Participants
				.Select(p => new ParticipantDtos
				{
					ParticipantId = p.ParticipantId,
					ParticipantName = p.ParticipantName
				})
				.ToListAsync();

			return participants;
		}

		public async Task<ParticipantDtos> GetParticipantByNameAsync(string participantName)
		{
			var participant = await dbContext.Participants
				.Where(p => p.ParticipantName.ToLower() == participantName.ToLower())
				.Select(p => new ParticipantDtos
				{
					ParticipantId = p.ParticipantId,
					ParticipantName = p.ParticipantName
				})
				.FirstOrDefaultAsync();

			return participant; // يمكن أن يكون null إذا لم يتم العثور عليه
		}

		public async Task<bool> AddoldParticipantToTeamAsync(int teamId, int participantId, DateTime year)
		{
			// البحث عن الفريق
			var team = await dbContext.Teams
				.Include(t => t.TeamsParticipant)
				.FirstOrDefaultAsync(t => t.TeamId == teamId);

			if (team == null)
				return false; // الفريق غير موجود

			// التأكد من أن الفريق لا يحتوي على أكثر من 3 مشاركين
			if (team.TeamsParticipant.Count >= 3)
				return false; // الفريق ممتلئ بالفعل

			// البحث عن المشارك باستخدام الـ ID
			var participant = await dbContext.Participants
				.FirstOrDefaultAsync(p => p.ParticipantId == participantId);

			if (participant == null)
				return false; // المشارك غير موجود

			// التأكد من أن المشارك غير مضاف مسبقًا إلى الفريق
			if (team.TeamsParticipant.Any(tp => tp.ParticipantId == participant.ParticipantId))
				return false; // المشارك مضاف بالفعل

			// إنشاء العلاقة بين الفريق والمشارك مع السنة
			var teamParticipant = new TeamParticipant
			{
				TeamId = teamId,
				ParticipantId = participantId,
				Year = year
			};

			dbContext.TeamsParticipants.Add(teamParticipant);
			await dbContext.SaveChangesAsync();

			return true; // تم إضافة المشارك بنجاح
		}


	}
}
