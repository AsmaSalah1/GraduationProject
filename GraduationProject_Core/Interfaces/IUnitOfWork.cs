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
		Task<int> SaveAsync(); //ال saveChanges
							   //بترجعلي عدد الاسطر الي تم اضافتها او حذفها او التعديل عليها ..
	}
}
