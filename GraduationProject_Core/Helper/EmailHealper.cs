using GraduationProject_Core.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Helper
{
	public class EmailHealper
	{
		public static void SendEmail(Email email)
		{
			try
			{
				var client = new SmtpClient("smtp.gmail.com", 587);
				client.EnableSsl = true;
				client.Credentials = new NetworkCredential("mustafaalrifaya3@gmail.com", "liyi eacp ozyb ryjx");
				client.Send("mustafaalrifaya3@gmail.com", email.Recivers, email.Subject, email.Body);
			}
			catch (Exception ex)
			{
				// هنا يمكن تسجيل الخطأ أو عرضه على الشاشة لتتبعه
				Console.WriteLine("Failed to send email: " + ex.Message);
			}
		}
	}
}
