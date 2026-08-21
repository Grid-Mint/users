using Users.Domain.Entities;

namespace Users.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task<IReadOnlyList<User>> GetAllAsync(Guid? venueId, int skip, int take, CancellationToken ct = default);
    Task<User> AddAsync(User user, CancellationToken ct = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct = default);
}