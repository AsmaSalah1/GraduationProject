using GraduationProject_Core.Dtos.Auth;
using Microsoft.Extensions.Configuration;
using MimeKit;
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
		private readonly IConfiguration _configuration;
		public EmailHealper(IConfiguration configuration)
		{
			_configuration = configuration;
		}
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
		public static string SendEmail2(Email email)
		{
			try
			{
				var client = new SmtpClient("smtp.gmail.com", 587)
				{
					EnableSsl = true,
					Credentials = new NetworkCredential(email.Sender, email.SenderPassword)
				};

				client.Send(email.Sender, email.Recivers, email.Subject, email.Body);
				
			}
			catch (Exception ex)
			{
			 return "Failed to send email: " + ex.Message;
			}
			return "Email sent successfully.";
		}
		//public async Task SendEmailAsync(string name, string userEmail, string message)
		//{
		//	var email = new MimeMessage;
		//	email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:SenderEmail"]));  // البريد الذي سيرسل منه الرسالة
		//	email.To.Add(MailboxAddress.Parse(_configuration["EmailSettings:AdminEmail"]));    // بريد الأدمن الذي سيستقبل الرسالة
		//	email.Subject = "New Contact Form Submission from " + name;

		//	email.Body = new TextPart("plain")
		//	{
		//		Text = $"👤 Name: {name}\n📧 Email: {userEmail}\n✉️ Message: {message}"
		//	};

		//	using var smtp = new SmtpClient();
		//	await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"],
		//							int.Parse(_configuration["EmailSettings:SmtpPort"]),
		//							true);
		//	await smtp.AuthenticateAsync(_configuration["EmailSettings:SenderEmail"],
		//								 _configuration["EmailSettings:SenderPassword"]);
		//	await smtp.SendAsync(email);
		//	await smtp.DisconnectAsync(true);
		//}

	}
}
