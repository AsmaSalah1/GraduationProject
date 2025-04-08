using GraduationProject_Core.Dtos.Team;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GraduationProject_Infrastructure.Repositories
{
	public class TeamRepositry : ITeamRepository
	{
		private readonly ApplicationDbContext dbContext;

		public TeamRepositry(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<bool> AddTeamAsync(CreateTeamDto dto, int competitionId, int userId)
		{
			var team = new Team
			{
				TeamName = dto.TeamName,
				UniversityName = dto.UniversityName,
				Coach = dto.Coach,
				TeamsParticipant = new List<TeamParticipant>()
			};

			if (dto.Participants.Count < 1 || dto.Participants.Count > 3 || dto.Participants == null)
				return false; // عدد المشاركين غير صحيح

			foreach (var participantDto in dto.Participants)
			{
				// تحقق إذا كان المشارك موجود باستخدام اسم المشارك
				var existingParticipant = await dbContext.Participants
					.FirstOrDefaultAsync(p => p.ParticipantName == participantDto.ParticipantName);

				if (existingParticipant == null)
				{
					// إذا لم يكن المشارك موجودًا، نضيفه جديدًا
					var participant = new Participant
					{
						ParticipantName = participantDto.ParticipantName
					};
					await dbContext.Participants.AddAsync(participant);
					await dbContext.SaveChangesAsync(); // حفظ المشارك الجديد

					// نربط المشارك بالفريق
					team.TeamsParticipant.Add(new TeamParticipant
					{
						TeamId = team.TeamId,
						ParticipantId = participant.ParticipantId,
						Year = dto.Year
					});
				}
				else
				{
					// إذا كان المشارك موجودًا، نربطه مباشرة بالفريق
					team.TeamsParticipant.Add(new TeamParticipant
					{
						TeamId = team.TeamId,
						ParticipantId = existingParticipant.ParticipantId,
						Year = dto.Year
					});
				}
			}

			await dbContext.Teams.AddAsync(team);
			await dbContext.SaveChangesAsync();

			// ربط الفريق بالمسابقة عبر الـ competitionId
			var teamCompetition = new TeamCompetition
			{
				TeamId = team.TeamId,
				CompetitionID = competitionId,
				Ranking = dto.Ranking,
				year = dto.Year.Year
			};

			await dbContext.TeamsCompetitions.AddAsync(teamCompetition);
			await dbContext.SaveChangesAsync();

			return true;
		}


		public async Task<IEnumerable<TeamsDetailsDto>> GetTeamsByCompetitionIdAsync(int competitionId)
		{
			// البحث عن الفرق المرتبطة بالمسابقة المحددة
			var teamCompetitions = await dbContext.TeamsCompetitions
				.Where(tc => tc.CompetitionID == competitionId) // تصفية الفرق حسب CompetitionID
				.Include(tc => tc.Team) // ربط تفاصيل الفريق
				.ThenInclude(t => t.TeamsParticipant) // ربط تفاصيل المشاركين
				.ThenInclude(tp => tp.Participant) // ربط تفاصيل كل مشارك
				.OrderBy(tc => tc.Ranking) // ترتيب الفرق حسب الـ Ranking
				.ToListAsync();

			// تحويل النتائج إلى DTO يعرض كل التفاصيل المطلوبة
			return teamCompetitions.Select(tc => new TeamsDetailsDto
			{
				Id=tc.TeamId,
				//TeamId = tc.TeamId, // إضافة الـ TeamId
				TeamName = tc.Team.TeamName,
				Ranking = tc.Ranking, // إضافة الـ Ranking
				UniversityName = tc.Team.UniversityName,
				Coach = tc.Team.Coach,
				Participants = tc.Team.TeamsParticipant.Select(tp => tp.Participant.ParticipantName).ToList() // أسماء المشاركين
			}).ToList();
		}

		public async Task<bool> LinkTeamToCompetitionAsync(int competitionId, int teamId, LinkTeamToCompetitionDto dto)
		{
			// تحقق مما إذا كان الفريق موجودًا
			var team = await dbContext.Teams.FindAsync(teamId);
			if (team == null)
			{
				return false; // الفريق غير موجود
			}

			// تحقق مما إذا كانت المسابقة موجودة
			var competition = await dbContext.Competitions.FindAsync(competitionId);
			if (competition == null)
			{
				return false; // المسابقة غير موجودة
			}

			// تحقق إذا كان الفريق قد تم ربطه مسبقًا مع هذه المسابقة
			var existingTeamCompetition = await dbContext.TeamsCompetitions
				.FirstOrDefaultAsync(tc => tc.TeamId == teamId && tc.CompetitionID == competitionId);

			if (existingTeamCompetition != null)
			{
				return false; // الفريق مرتبط مسبقًا بهذه المسابقة
			}

			// ربط الفريق مع المسابقة
			var teamCompetition = new TeamCompetition
			{
				TeamId = teamId,
				CompetitionID = competitionId,
				Ranking = dto.Ranking,
				year = dto.Year
			};

			await dbContext.TeamsCompetitions.AddAsync(teamCompetition);
			await dbContext.SaveChangesAsync();

			return true; // تم الربط بنجاح
		}


		public async Task<bool> DeleteTeamAsync(int teamId, int userId)
		{
			var team = await dbContext.Teams
							.Include(t => t.TeamsParticipant)
							.FirstOrDefaultAsync(t => t.TeamId == teamId);

			if (team == null)
			{
				return false; // الفريق غير موجود
			}

			dbContext.Teams.Remove(team);
			await dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<IEnumerable<TeamDetailsDto>> GetAllTeamsAsync()
		{
			var teams = await dbContext.Teams
				.Include(t => t.TeamsParticipant)
				.ThenInclude(tp => tp.Participant)
				.ToListAsync();

			return teams.Select(team => new TeamDetailsDto
			{
				TeamId = team.TeamId, // إضافة TeamId هنا
				TeamName = team.TeamName,
				UniversityName = team.UniversityName,
				Coach = team.Coach,
				Participants = team.TeamsParticipant.Select(tp => tp.Participant.ParticipantName).ToList()
			}).ToList();
		}


		public async Task<bool> UpdateTeamAsync(int competitionId, int teamId, UpdateTeamDto dto, int userId)
		{
			var team = await dbContext.Teams
				.Include(t => t.TeamsCompetitions) // تأكد من تضمين TeamsCompetitions
				.FirstOrDefaultAsync(t => t.TeamId == teamId);

			if (team == null)
				return false;

			// تحديث اسم الفريق
			if (!string.IsNullOrEmpty(dto.TeamName))
				team.TeamName = dto.TeamName;

			// تحديث اسم الجامعة
			if (!string.IsNullOrEmpty(dto.UniversityName))
				team.UniversityName = dto.UniversityName;

			// تحديث اسم المدرب
			if (!string.IsNullOrEmpty(dto.Coach))
				team.Coach = dto.Coach;

			// تحديث الترتيب إذا تم إرسال الـ Ranking و CompetitionId
			if (dto.Ranking.HasValue)
			{
				var teamCompetition = team.TeamsCompetitions
					.FirstOrDefault(tc => tc.CompetitionID == competitionId);  // نستخدم الـ competitionId الذي وصلنا من البرامتر

				if (teamCompetition != null)
				{
					teamCompetition.Ranking = dto.Ranking.Value;  // تحديث الترتيب
				}
				else
				{
					// إذا لم يتم العثور على المسابقة، يمكن إضافة سجل جديد
					var newTeamCompetition = new TeamCompetition
					{
						TeamId = teamId,
						CompetitionID = competitionId,  // استخدام الـ competitionId الذي وصلنا من البرامتر
						Ranking = dto.Ranking.Value,
						year = DateTime.Now.Year  // أو يمكنك تعديل السنة حسب الحاجة
					};
					await dbContext.TeamsCompetitions.AddAsync(newTeamCompetition);  // إضافة السجل الجديد
				}
			}

			// حفظ التعديلات في قاعدة البيانات
			await dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<IEnumerable<TeamsDetailsDto>> GetTeamsByParticipantNameAsync(string participantName)
		{
			var teams = await dbContext.Teams
//.Where(t => EF.Functions.Like(t.TeamName.ToLower(), teamName.ToLower() + "%")) // التأكد من عدم حساسية حالة الأحرف

               .Where(t => t.TeamsParticipant.Any(tp => tp.Participant.ParticipantName.ToLower().StartsWith(participantName.ToLower())))
				.Include(t => t.TeamsParticipant) // ربط المشاركين
				.ThenInclude(tp => tp.Participant)
				.Include(t => t.TeamsCompetitions) // ربط الفرق بالمسابقات للحصول على الرانك
				.ThenInclude(tc => tc.Competition) // يمكن ربط المسابقة أيضاً إذا أردت تفاصيل أكثر
				.ToListAsync();

			return teams.Select(t => new TeamsDetailsDto
			{
				TeamName = t.TeamName,
				UniversityName = t.UniversityName,
				Coach = t.Coach,
				Participants = t.TeamsParticipant.Select(tp => tp.Participant.ParticipantName).ToList(),
				Ranking = t.TeamsCompetitions.FirstOrDefault()?.Ranking ?? 0 // إرجاع أول ترتيب موجود أو صفر إذا لم يكن موجود
			}).ToList();
		}

		public async Task<bool> RemoveTeamFromCompetitionAsync(int competitionId, int teamId)
		{
			// البحث عن السجل الذي يربط الفريق بالمسابقة
			var teamCompetition = await dbContext.TeamsCompetitions
				.FirstOrDefaultAsync(tc => tc.TeamId == teamId && tc.CompetitionID == competitionId);

			if (teamCompetition == null)
			{
				return false; // إذا لم يتم العثور على السجل، يعني الفريق غير مرتبط بالمسابقة
			}

			// حذف السجل الذي يربط الفريق بالمسابقة
			dbContext.TeamsCompetitions.Remove(teamCompetition);
			await dbContext.SaveChangesAsync();
			return true; // تم الحذف بنجاح
		}
		public async Task<IEnumerable<TeamsDetailsDto>> GetTeamsByTeamNameAsync(string teamName)
		{
			var teams = await dbContext.Teams
		//.Where(t => EF.Functions.Like(t.TeamName.ToLower(), teamName.ToLower() + "%")) // التأكد من عدم حساسية حالة الأحرف
				.Where(t => t.TeamName.Contains(teamName)) // البحث عن الفرق التي تحتوي على اسم الفريق المدخل
				.Include(t => t.TeamsParticipant) // ربط المشاركين
				.ThenInclude(tp => tp.Participant)
				.Include(t => t.TeamsCompetitions) // ربط الفرق بالمسابقات للحصول على الرانك
				.ThenInclude(tc => tc.Competition) // يمكن ربط المسابقة أيضاً إذا أردت تفاصيل أكثر
				.ToListAsync();
			var query = dbContext.Teams
				.Where(t => EF.Functions.Like(t.TeamName, teamName + "%"));

			Console.WriteLine(query.ToQueryString()); // طباعة الاستعلام الذي يولده EF Core

			return teams.Select(t => new TeamsDetailsDto
			{
				TeamName = t.TeamName,
				UniversityName = t.UniversityName,
				Coach = t.Coach,
				Participants = t.TeamsParticipant.Select(tp => tp.Participant.ParticipantName).ToList(),
				Ranking = t.TeamsCompetitions.FirstOrDefault()?.Ranking ?? 0 // إرجاع أول ترتيب موجود أو صفر إذا لم يكن موجود
			}).ToList();
		}

	}
}
