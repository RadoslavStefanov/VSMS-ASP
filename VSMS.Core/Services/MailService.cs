using System.Net.Mail;
using System.Text;
using System.Text.Json;
using VSMS.Core.ViewModels;

namespace VSMS.Core.Services
{
    public class MailService
    {
        public bool SendSalesReport(string JSON)
        {
            MailMessage message = new MailMessage();
            message.To.Add("danielastefanova81@abv.bg");
            message.To.Add("gri6ka@abv.bg");
            message.To.Add("radoslav.g.stefanov@gmail.com");

            message.From= new MailAddress("vsms.alerts @gmail.com");

            var textResult = JSONSerializeToText(JSON.Split("*_*")[0]);
            var mailbody = new StringBuilder();
            mailbody.AppendLine($"Здравейте VSMS Ви предоставя отчет \"Продажби по количества\" за дата: {DateTime.Today.ToString().Split(" ")[0]}");
            mailbody.AppendLine($"{JSON.Split("*_*")[1]}");
            mailbody.AppendLine($"{textResult}"); 
            message.Subject = $"Продажби по количества {DateTime.Today.ToString().Split(" ")[0]}";
            var test = mailbody.ToString();
            message.Body = (mailbody.ToString());
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = false;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("vsms.alerts@gmail.com", "Rado-2505");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;

            try
            { client.Send(message); return true; }
            catch (Exception)
            { return false; }
        }

        private string JSONSerializeToText(string JsonInput)
        {
            var desResult = JsonSerializer.Deserialize<List<MailReportViewModel>>(JsonInput);
            if (desResult.Count == 0)
            { return ""; }

            var output = new StringBuilder();
            foreach (var item in desResult)
            {output.AppendLine($"\n{item.productName} | {item.soldAmount} | {item.totalPrice}");}

            return output.ToString();
        }
    }
}
