using GraduationProject_Core.Dtos.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
	public interface IRuleRepositry
	{
		Task<string> AddRuleAsync(int userId, AddRuleDto dto);
		Task<IEnumerable<AddRuleDto>> GetAllRulesAsync();
		Task<string> EditRuleAsync(int userId,int RuleId,EditRuleDto dto);
		Task<string> DeleteRuleAsync(int userId, int ruleId);

	}
}
