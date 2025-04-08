using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Helper
{
	public class FileHelper
	{
		public static string UplodeFile(IFormFile file, string folderName )
		{
			// تحديد مسار التخزين داخل wwwroot
			string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

			// التأكد من وجود المجلد وإنشاؤه إذا لم يكن موجودًا
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			// إنشاء اسم فريد للملف
			string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

			// تحديد المسار الكامل للملف
			string filePath = Path.Combine(folderPath, fileName);

			// رفع الملف إلى المجلد
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				file.CopyTo(fileStream);
			}

			// إرجاع المسار النسبي (لتخزينه في قاعدة البيانات)
			return $"/{folderName}/{fileName}";
		}
		//public static string UplodeFile(IFormFile file, string folderName)
		//{
		//	string floderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
		//	/*                C: \Users\user\Desktop\asp\seesion1\Asmaa\Asma.pl\wwwroot\files\Images\
		//           *                C:\Users\user\Desktop\asp\seesion1\Asmaa\Asma.pl\wwwroot\files\Images\
		//          */
		//	string fileName = $"{Guid.NewGuid()}{file.FileName}";
		//	string filePath = Path.Combine(floderPath, fileName);
		//	using (var fileStream = new FileStream(filePath, FileMode.Create))
		//	{
		//		file.CopyTo(fileStream);
		//	}
		//	return fileName;

		//}
		public static void DeleteFile(string fileName, string folderName)
		{
			string filePath = Path.Combine("C:\\Users\\user\\Desktop\\asp\\GraduationProject_API\\GraduationProject_API\\wwwroot\\Cvs\\",  fileName);
			Console.WriteLine(filePath);
			filePath.Replace( "\\" , "/");
			Console.WriteLine(filePath);
			if (System.IO.File.Exists(filePath))
			{

				System.IO.File.Delete(filePath);
			}
		}//why?? :)
	}
}
