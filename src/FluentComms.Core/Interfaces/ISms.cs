namespace FluentComms.Core.Interfaces
{
    public interface ISms : IMessage
    {
        string From { get; set; }

        string To { get; set; }

        string Message { get; set; }
    }
}
