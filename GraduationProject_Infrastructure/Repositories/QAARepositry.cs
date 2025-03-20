using Azure.Core;
using GraduationProject_Core.Dtos.QAA;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Infrastructure.Repositories
{
	public class QAARepositry : IQAARepositry
	{
		private readonly ApplicationDbContext dbContext;

		public QAARepositry(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<bool> Add_QAA_Async(int userId, CreateQAADto dto)
		{
			var QaA = new QAA()
			{
				UserID = userId,
				Question=dto.Question,
				Answer=dto.Answer,
			};
			var result= await dbContext.AddAsync(QaA);
			await dbContext.SaveChangesAsync();
			return true;
		}
		public async Task<bool> Update_QAA_Async(int qaaId, int userId, UpdateQAADto dto)
		{
			var qaa = await dbContext.QAAs.FindAsync(qaaId);
			if (qaa == null || qaa.UserID != userId)
			{
				return false; // السؤال غير موجود أو ليس مملوكًا لهذا المستخدم
			}
			if(!string.IsNullOrEmpty(dto.Question))
			{
			qaa.Question = dto.Question;
			}
			if (!string.IsNullOrEmpty(dto.Answer))
			{
				qaa.Answer = dto.Answer;

			}
			await dbContext.SaveChangesAsync();
			return true; 
		}
		public async Task<bool> Delete_QAA_Async(int userId, int qaaId)
		{
			var qaa = await dbContext.QAAs.FirstOrDefaultAsync(q => q.QAAID == qaaId && q.UserID == userId);

			if (qaa == null)
			{
				return false; // لم يتم العثور على السؤال أو المستخدم غير مخوّل
			}

			dbContext.QAAs.Remove(qaa);
			await dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<IEnumerable<CreateQAADto>> GetAll_QAA_Async()
		{
			var result = await dbContext.QAAs.AsNoTracking().ToListAsync();
			var Dto =result.Select(x => new CreateQAADto()
			{
				Answer = x.Answer,
				Question = x.Question,
			});
			return Dto;
		}
	}
}
