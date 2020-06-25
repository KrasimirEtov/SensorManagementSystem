using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.Services
{
	public class EmailService : IEmailService
	{
		private readonly string senderEmailAddress = Environment.GetEnvironmentVariable("SensorManagementSystemEmailSender", EnvironmentVariableTarget.User);
		private readonly string senderEmailPassword = Environment.GetEnvironmentVariable("SensorManagementSystemEmailPassword", EnvironmentVariableTarget.User);
		private readonly string smtp = "smtp.gmail.com";
		private readonly int smtpPort = 587;
		private readonly ILogger<EmailService> _logger;

		public EmailService(ILogger<EmailService> logger)
		{
			_logger = logger;
		}

		public async Task SendAsync(string receiver, string subject, string body)
		{
			if (string.IsNullOrEmpty(receiver) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))

			{
				throw new ArgumentNullException("Receiver email address, email subject or email body cannot be null");
			}

			try
			{
				var message = GenerateMessage(receiver, subject, body);

				using var client = new SmtpClient();
				client.Connect(smtp, smtpPort, false);
				client.AuthenticationMechanisms.Remove("XOAUTH2");

				await client.AuthenticateAsync(senderEmailAddress, senderEmailPassword);
				await client.SendAsync(message);
				await client.DisconnectAsync(true);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				throw;
			}
		}

		private MimeMessage GenerateMessage(string receiverEmailAddress, string emailSubject, string emailBody)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("SensorManagementSystem", senderEmailAddress));

			message.To.Add(new MailboxAddress("SensorManagementSystem", receiverEmailAddress));

			message.Subject = emailSubject;

			message.Body = new TextPart("plain")
			{
				Text = emailBody
			};

			return message;
		}
	}
}
