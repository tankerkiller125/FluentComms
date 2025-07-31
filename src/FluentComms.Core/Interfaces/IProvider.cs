using System.Threading.Tasks;

namespace FluentComms.Core.Interfaces
{
    public interface IProvider
    {
        Task<ISenderResult> SendAsync(IMessage message);
        ISenderResult Send(IMessage message);
    }
}
