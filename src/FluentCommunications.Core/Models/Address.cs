using FluentCommunications.Core.Interfaces;

namespace FluentCommunications.Core.Models
{
    public class Address : IAddress
    {
        public string? Name { get; set; }

        public string Email { get; set; }

        public Address(string email, string? name = null)
        {
            Email = email;
            Name = name;
        }
    }
}
