using RestApi.Dtos;
using RestApi.Models;

namespace RestApi.Services;

public interface IGroupService{
    public Task<GroupUserModel> GetGroupByIdAsync(string id, CancellationToken cancellationToken);
    public Task<IEnumerable<GroupUserModel>> GetGroupsByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken);
}