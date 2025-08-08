using System.Threading.Tasks;
using FluentComms.Core.Interfaces;

namespace FluentComms.Core.Channels
{
    public class SmsChannel : IChannel<ISms>
    {
        private readonly IProvider _provider;

        public SmsChannel(IProvider provider)
        {
            _provider = provider;
        }

        public async Task<ISenderResult> SendAsync(ISms message)
        {
            return await _provider.SendAsync(message);
        }

        public ISenderResult Send(ISms message)
        {
            return _provider.Send(message);
        }
    }
}