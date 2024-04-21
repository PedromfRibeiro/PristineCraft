using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Extensions;

public static class FluentEmailExtensions
{
    public static void AddFluentEmail(this IServiceCollection services, ConfigurationManager configuration)
    {
        var emailSettings = configuration.GetSection("SMTPSetting");

        //TODO: Null Check
        SmtpClient smtp = new SmtpClient()
        {
            Port = emailSettings.GetValue<int>("Port"),
            Host = emailSettings["Host"] ?? "",
            EnableSsl = Convert.ToBoolean(emailSettings["EnableSsl"]),
            UseDefaultCredentials = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(emailSettings["UserName"], emailSettings["Password"]),
        };
        services.AddFluentEmail(emailSettings["DefaultFromEmail"]).AddSmtpSender(smtp);
    }
}