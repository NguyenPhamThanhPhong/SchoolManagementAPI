using SchoolManagementAPI.Services.Configs;
using System.Net.Mail;
using System.Net;

namespace SchoolManagementAPI.Services.SMTP
{
    public class EmailUtil
    {
        private readonly SMTPConfig _SMTPConfigs;

        public EmailUtil(SMTPConfig sMTPConfigs)
        {
            _SMTPConfigs = sMTPConfigs;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string titleEmail, string contentEmail)
        {
            // Thay đổi thông tin tài khoản Gmail của bạn tại đây
            string senderEmail = "phong741258963@gmail.com";
            string password = "pvdpwkkcyoniutve";

            // Cấu hình thông tin kết nối SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, password);

            // Tạo đối tượng MailMessage để cấu hình email
            MailMessage mailMessage = new MailMessage(senderEmail, toEmail, titleEmail, contentEmail);
            mailMessage.IsBodyHtml = true;

            try
            {
                // Gửi email
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error is");
                Console.WriteLine(ex.Message);
                Console.WriteLine("end ------------");
                return false;
            }
        }
    }
}
