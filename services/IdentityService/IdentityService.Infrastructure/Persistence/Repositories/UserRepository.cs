using IdentityService.Application.Common.Interfaces;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _ctx;

    public UserRepository(IdentityDbContext ctx) => _ctx = ctx;

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        => _ctx.Users
               .FirstOrDefaultAsync(u => u.Email == email.ToLower(), ct);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        => _ctx.Users.FindAsync(new object[] { id }, ct).AsTask();

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
        => _ctx.Users.AnyAsync(u => u.Email == email.ToLower(), ct);

    public async Task AddAsync(User user, CancellationToken ct)
        => await _ctx.Users.AddAsync(user, ct);

    public void Update(User user)
        => _ctx.Users.Update(user);
}