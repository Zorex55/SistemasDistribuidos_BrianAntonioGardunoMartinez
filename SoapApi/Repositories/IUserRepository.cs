using SoapApi.Models;

namespace SoapApi.Repositories;

public interface IUserRepository{ 
    public Task<UserModel> GetByIdAsync(Guid UserId, CancellationToken cancellationToken);
    Task<IList<UserModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<IList<UserModel>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    public Task DeleteByIdAsync(UserModel user, CancellationToken cancellationToken);
    public Task<UserModel> CreateAsync(UserModel user, CancellationToken cancellationToken);
    public Task<bool> UpdateAsync(Guid userId, string firstName, string lastName, DateTime birthday, CancellationToken cancellationToken);
}