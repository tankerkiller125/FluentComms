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

    // Shorter alias to match the problem statement examples
    public static class FluentComms
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
