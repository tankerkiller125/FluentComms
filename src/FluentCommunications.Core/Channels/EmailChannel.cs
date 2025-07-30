using System.Threading.Tasks;
using FluentCommunications.Core.Interfaces;

namespace FluentCommunications.Core.Channels
{
    public class EmailChannel : IChannel<IEmail>
    {
        private readonly IProvider _provider;

        public EmailChannel(IProvider provider)
        {
            _provider = provider;
        }

        public async Task<ISenderResult> SendAsync(IEmail message)
        {
            return await _provider.SendAsync(message);
        }

        public ISenderResult Send(IEmail message)
        {
            return _provider.Send(message);
        }
    }
}
