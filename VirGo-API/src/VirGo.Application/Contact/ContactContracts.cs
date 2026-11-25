using VirGo.Domain.Contact;

namespace VirGo.Application.Contact;

public interface IContactMessageRepository
{
    Task AddAsync(ContactMessage message, CancellationToken cancellationToken = default);
}
