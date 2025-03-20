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

		public static string UplodeFile(IFormFile file, string folderName)
		{
			string floderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
			/*                C: \Users\user\Desktop\asp\seesion1\Asmaa\Asma.pl\wwwroot\files\Images\
             *                C:\Users\user\Desktop\asp\seesion1\Asmaa\Asma.pl\wwwroot\files\Images\
            */
			string fileName = $"{Guid.NewGuid()}{file.FileName}";
			string filePath = Path.Combine(floderPath, fileName);
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				file.CopyTo(fileStream);
			}
			return fileName;

		}
		public static void DeleteFile(string fileName, string folderName)
		{
			string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName, fileName);
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}
	}
}
