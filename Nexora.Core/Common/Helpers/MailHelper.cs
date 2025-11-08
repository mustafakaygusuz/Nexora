using Nexora.Core.Common.Configurations;
using Nexora.Core.Common.Extensions;
using System.Net;
using System.Net.Mail;

namespace Nexora.Core.Common.Helpers
{
    public static class MailHelper
    {
        public static async Task<(bool Success, string? Exception)> SendMail(MailSmtpConfigurationModel configuration, string? subject, List<string> toList, string message, string? imageAttachment = null, List<string>? ccList = null, List<string>? bccList = null, byte[]? excelAttachment = null, string? excelFileName = null)
        {
            try
            {
                var credentials = new NetworkCredential(configuration.Email, configuration.Password);

                var mail = new MailMessage()
                {
                    From = new MailAddress(configuration.Email!),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                if (imageAttachment.HasValue())
                {
                    var imageBytes = Convert.FromBase64String(imageAttachment!);

                    var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                    mail.Attachments.Add(new Attachment(ms, "image.png", "image/png"));
                }

                if (excelAttachment.HasValue())
                {
                    var ms = new MemoryStream(excelAttachment!, 0, excelAttachment!.Length);

                    mail.Attachments.Add(new Attachment(ms, excelFileName ?? "excel.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
                }

                AddMailAddresses(mail.To, toList);
                AddMailAddresses(mail.CC, ccList);
                AddMailAddresses(mail.Bcc, bccList);

                using (var client = new SmtpClient { Port = configuration.Port ?? 0, DeliveryMethod = SmtpDeliveryMethod.Network, UseDefaultCredentials = false, Host = configuration.Server!, EnableSsl = true, Credentials = credentials })
                {
                    await client.SendMailAsync(mail);
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private static void AddMailAddresses(MailAddressCollection addressCollection, List<string>? addresses)
        {
            if (addresses.HasValue())
            {
                addresses!.ForEach(x =>
                {
                    addressCollection.Add(new MailAddress(x));
                });
            }
        }
    }
}