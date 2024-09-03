using SoapApi.Models;

namespace SoapApi.Repositories;

public interface IUserRepository{ 
    public Task<UserModel> GetByIdAsync(Guid UserId, CancellationToken cancellationToken);
    Task<IList<UserModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<IList<UserModel>> GetByEmailAsync(string email, CancellationToken cancellationToken);
}