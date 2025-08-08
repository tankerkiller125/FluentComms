using FluentComms.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentComms.Providers.MailKit
{
    public static class FluentCommsBuilderExtensions
    {
        public static FluentCommsBuilder UseMailKitSmtp(this FluentCommsBuilder builder, Action<MailKitSmtpOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddScoped<MailKitSmtpSender>();
            builder.AddSender<MailKitSmtpSender>();
            return builder;
        }
    }
}
