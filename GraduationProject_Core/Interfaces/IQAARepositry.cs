using GraduationProject_Core.Dtos.QAA;
using GraduationProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
	public interface IQAARepositry
	{
		Task<IEnumerable<CreateQAADto>> GetAll_QAA_Async();
		Task<bool> Add_QAA_Async(int userId, CreateQAADto dto);
		Task<bool> Update_QAA_Async(int qaaId, int userId, UpdateQAADto dto);
		Task<bool> Delete_QAA_Async(int userId, int qaaId); // إضافة  الحذف

	}
}
