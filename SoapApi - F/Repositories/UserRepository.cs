using Microsoft.EntityFrameworkCore;
using SoapApi.Infraestructure;
using SoapApi.Models;
using SoapApi.Mappers;

namespace SoapApi.Repositories;

public class UserRepository : IUserRepository
{

    private readonly RelationalDbContext _dbContext;

    public UserRepository(RelationalDbContext dbContext){
        _dbContext = dbContext;
    }

    public async Task<IList<UserModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking().ToListAsync(cancellationToken);
        return user.Select(user => user.ToModel()).ToList();
    }

    public async Task<IList<UserModel>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking().Where(user => user.Email.Contains(email)).ToListAsync(cancellationToken);
        return user.Select(user => user.ToModel()).ToList();
    }

    public async Task<UserModel> GetByIdAsync(Guid UserId, CancellationToken cancellationToken){
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == UserId, cancellationToken);
        return user.ToModel();
  }
}