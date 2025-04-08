using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
	public interface IUnitOfWork
	{
		IAuthRepositry authRepositry { get; }
		IUserProfileRepositry userProfileRepositry { get; }
		IQAARepositry iQAARepositry { get; }
		IRuleRepositry iRuleRepositry { get; }
		IPersonalExperianceRepositry personalExperianceRepositry { get; }
		ITeamRepository teamRepository { get; }
		IParticipantRepository iparticipantRepository { get; }
		IPostRepositry iPostRepositry { get; }
		ICommentRepositry iCommentRepositry { get; }
		ILikeRepository iLikeRepository { get; }
		ICompetitionRepository iCompetitionRepository { get; }

		ICompetitionImagesRepository iCompetitionImagesRepository { get; }
		ISponsorRepository iSponsorRepository { get; }
		IUniversityRepositry iUniversityRepositry { get; }
		IUniversityImagesRepository iUniversityImagesRepository { get; }
		Task<int> SaveAsync(); //ال saveChanges
							   //بترجعلي عدد الاسطر الي تم اضافتها او حذفها او التعديل عليها ..
	}
}
