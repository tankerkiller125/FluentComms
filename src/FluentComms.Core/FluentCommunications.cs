using FluentComms.Core.Models;
using FluentComms.Core.Builders;

namespace FluentComms.Core
{
    public static class FluentCommunications
    {
        public static EmailBuilder Email()
        {
            return new EmailBuilder();
        }

        public static SmsBuilder Sms()
        {
            return new SmsBuilder();
        }
    }
}
