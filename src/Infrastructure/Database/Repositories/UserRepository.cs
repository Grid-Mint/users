using System;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Repositories;

namespace Users.Infrastructure.Database.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User> AddAsync(User user, CancellationToken ct = default)
    {
        var userEntry = context.Users.Add(user);
        await context.SaveChangesAsync(ct);
        return userEntry.Entity;
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    {
        return await context.Users.AnyAsync(u => u.Email == email, ct);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(Guid? id, int skip, int take, CancellationToken ct = default)
    {
        return await context.Users
            .Where(u => !id.HasValue || u.Id == id.Value)
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(u => u.SetProperty(user => user.IsDeleted, true), ct) > 0;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken ct = default)
    {
        var userEntry = context.Users.Update(user);
        await context.SaveChangesAsync(ct);
        return userEntry.Entity;
    }
}
