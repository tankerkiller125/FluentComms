using FluentComms.Core.Builders;

namespace FluentComms
{
    /// <summary>
    /// Main entry point for FluentComms API that provides a fluent interface for sending communications.
    /// </summary>
    public static class FluentComms
    {
        /// <summary>
        /// Creates a new email builder for composing and sending emails.
        /// </summary>
        /// <returns>An EmailBuilder instance for fluent email composition</returns>
        public static EmailBuilder Email()
        {
            return new EmailBuilder();
        }

        /// <summary>
        /// Creates a new SMS builder for composing and sending SMS messages.
        /// </summary>
        /// <returns>An SmsBuilder instance for fluent SMS composition</returns>
        public static SmsBuilder Sms()
        {
            return new SmsBuilder();
        }
    }
}