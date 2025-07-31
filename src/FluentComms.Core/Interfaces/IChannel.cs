using System.Threading.Tasks;

namespace FluentComms.Core.Interfaces
{
    public interface IChannel<T> where T : IMessage
    {
        Task<ISenderResult> SendAsync(T message);
        ISenderResult Send(T message);
    }
}
