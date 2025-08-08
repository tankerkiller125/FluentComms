namespace FluentComms.Providers.MailKit
{
    /// <summary>
    /// Configuration options for the MailKit SMTP sender.
    /// </summary>
    public class MailKitSmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; } = 587;
        public bool UseSsl { get; set; } = true;
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
