using RestApi.Models;

namespace RestApi.Repositories;

public interface IGroupRepository{
    Task<GroupModel> GetByIdAsync(string Id, CancellationToken cancellationToken);
    Task<IEnumerable<GroupModel>> GetByNameAsync(string name, int PageIndex, int PageSize, string orderBy, CancellationToken cancellationToken);

    public Task DeleteByIdAsync(string Id, CancellationToken cancellationToken);

    public Task<GroupModel> CreateAsync(string name, Guid[] users, CancellationToken cancellationToken);

    public Task<GroupModel> GetByExactNameAsync (string name, CancellationToken cancellationToken); //Creamos otra interfaz para no confundirla con la que ya está
}
