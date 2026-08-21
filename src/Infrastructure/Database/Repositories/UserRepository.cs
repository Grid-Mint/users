using System;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Enums;
using Users.Domain.Repositories;

namespace Users.Infrastructure.Database.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public Task<User> AddAsync(User user, CancellationToken ct = default)
        => Task.FromResult(context.Users.Add(user).Entity);

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
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

        if (user is null) return false;

        user.Status = Statuses.Inactive;
        return true;
    }

    public async Task<bool> UpdateRoleAsync(Guid id, Roles role, CancellationToken ct = default)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

        if (user is null) return false;

        user.Role = role;
        return true;
    }

    public async Task<bool> UpdateStatusAsync(Guid id, Statuses status, CancellationToken ct = default)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

        if (user is null) return false;

        user.Status = status;
        return true;
    }
}
