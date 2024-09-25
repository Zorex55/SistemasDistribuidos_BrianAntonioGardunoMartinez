using RestApi.Models;
using RestApi.Repositories;

namespace RestApi.Services;

public class GroupService : IGroupService{

    private readonly IGroupRepository _groupRepository;

    public GroupService(IGroupRepository groupRepository){ //Otra inyección de dependencias
        _groupRepository = groupRepository;
    }
    public async Task<GroupUserModel> GetGroupByIdAsync(string id, CancellationToken cancellationToken){
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        if (group is null){
            return null;
        }

        return new GroupUserModel{
            Id = group.Id,
            Name = group.Name,
            CreationTime = group.CreationDate
        };
    }
}