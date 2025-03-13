using GraduationProject_Core.Dtos.Rule;
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
	public class RuleRepositry : IRuleRepositry
	{
		private readonly ApplicationDbContext dbContext;

		public RuleRepositry(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<string> AddRuleAsync(int userId, AddRuleDto dto)
		{
			if(dto == null)
			{
				return "Entered data dose not Invalid";
			}

			var rule = new Rule()
			{
				UserID=userId,
				Description = string.Join("\n", dto.Description),
				Title = dto.Title,
			};
			var result = await dbContext.Rules.AddAsync(rule);
			await dbContext.SaveChangesAsync();
			if(result != null)
			{

				return "Rule Added Successfully";
			}
			return (result.ToString());
		}

		public async Task<string> EditRuleAsync(int userId, int RuleId, EditRuleDto dto)
		{

			if (dto == null)
			{
				return "Entered data is invalid";
			}

			var rule = await dbContext.Rules.FindAsync(RuleId);
			if (rule == null)
			{
				return "Rule not found";
			}

			// التأكد من أن المستخدم الذي يقوم بالتعديل هو نفس المستخدم الذي أضاف القاعدة (إذا كانت هناك حاجة للتحقق من هذا)
			if (rule.UserID != userId)
			{
				return "You are not authorized to update this rule";
			}

			// تحديث العنوان إذا كان موجودًا
			if (!string.IsNullOrEmpty(dto.Title))
			{
				rule.Title = dto.Title;
			}

			// تحديث الوصف إذا كان موجودًا
			if (dto.Description != null && dto.Description.Any())
			{
				rule.Description = string.Join("\n", dto.Description); // تحويل الـ List<string> إلى string
			}

			dbContext.Rules.Update(rule);
			await dbContext.SaveChangesAsync();

			return "Rule updated successfully";
		}

		public async Task<IEnumerable<AddRuleDto>> GetAllRulesAsync()
		{
			var rule = await dbContext.Rules.AsNoTracking().ToListAsync();
			var result = rule.Select(x => new AddRuleDto()
			{
				// تحويل النص إلى قائمة
				Description = x.Description.Split("\n").ToList(),
				Title = x.Title,
			});
			if (result != null)
			{
				return result;
			}
			return null;
		}

		public async Task<string> DeleteRuleAsync(int userId, int ruleId)
		{
			var rule = await dbContext.Rules.FindAsync(ruleId);
			if (rule == null)
			{
				return "Rule not found";
			}

			// التأكد من أن المستخدم الذي يقوم بالحذف هو نفس المستخدم الذي أضاف القاعدة)
			if (rule.UserID != userId)
			{
				return "You are not authorized to delete this rule";
			}

			dbContext.Rules.Remove(rule);
			await dbContext.SaveChangesAsync();

			return "Rule deleted successfully";
		}

	}
}
