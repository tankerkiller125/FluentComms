using FluentComms.Core.Models;

namespace FluentComms.Core
{
    public static class FluentCommunications
    {
        public static Email Email()
        {
            return new Email();
        }

        public static Sms Sms()
        {
            return new Sms();
        }
    }
}
