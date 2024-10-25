using RestApi.Models;

namespace RestApi.Repositories;

public interface IUserRepository{
    Task<UserModel> GetByIdAsync(Guid UserId, CancellationToken cancellationToken);
}