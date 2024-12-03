using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Exceptions;
using RestApi.Infraestructure.Soap;
using RestApi.Models;
using RestApi.Repositories;

namespace RestApi.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;

    private readonly IUserRepository _userRepository;

    public GroupService(IGroupRepository groupRepository, IUserRepository userRepository)
    {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public async Task<GroupUserModel> CreateGroupAsync(string name, Guid[] users, int PageSize, int PageIndex, string orderBy, CancellationToken cancellationToken){
        if(users.Length == 0){
            throw new InvalidGroupRequestFormatException();
        }

        var groups = await _groupRepository.GetByNameAsync(name, PageSize, PageIndex, orderBy, cancellationToken);
        if(groups.Any()){
            throw new GroupAlreadyExistsException();
        }
        foreach(var userId in users){
            var user = _userRepository.GetByIdAsync(userId, cancellationToken);
            if(user != null){
                throw new IdAlreadyExistsException();
            }
        }
        var group = await _groupRepository.CreateAsync(name, users, cancellationToken);
        
        return new GroupUserModel {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationTime,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user != null).ToList()
        };
    }

    public async Task DeleteGroupByIdAsync(string Id, CancellationToken cancellationToken){
        var group = await _groupRepository.GetByIdAsync(Id, cancellationToken);
        if (group is null){
            throw new GroupNotFoundException();
        }
        await _groupRepository.DeleteByIdAsync(Id, cancellationToken);
    }

    public async Task<GroupUserModel> GetGroupByIdAsync(string Id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(Id, cancellationToken);

        if(group == null)
        {
            return null;
        }

        return new GroupUserModel {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationTime,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user != null).ToList()
        };
    }
    
    public async Task<IEnumerable<GroupUserModel>> GetGroupsByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken)
    {
        var groups = await _groupRepository.GetByNameAsync(name, pageIndex, pageSize, orderBy, cancellationToken);

        var groupUserModels = await Task.WhenAll(groups.Select(async group => 
        {
            var users = await Task.WhenAll(group.Users.Select(Id => _userRepository.GetByIdAsync(Id, cancellationToken)));
            return new GroupUserModel
            {
                Id = group.Id,
                Name = group.Name,
                CreationDate = group.CreationTime,
                Users = users.Where(user => user != null).ToList()
            };
        }));

        var orderedGroups = orderBy switch
        {
            "name" => groupUserModels.OrderBy(g => g.Name),
            "creationDate" => groupUserModels.OrderBy(g => g.CreationDate),
            _ => groupUserModels.OrderBy(g => g.Name)
        };

        return orderedGroups
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public async Task UpdateGroups(string id, string name, Guid[] users, CancellationToken cancellationToken){
        if (users.Length == 0){
            throw new InvalidGroupRequestFormatException();
        }

        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        if(group is null){
            throw new GroupNotFoundException();
        }

        var groups = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
        if(groups != null && groups.Id != id){
            throw new GroupAlreadyExistsException();
        }

        foreach(var userId in users){
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if(user != null){
                throw new IdAlreadyExistsException();
            }
        }
        await _groupRepository.UpdateGroupAsync(id, name, users, cancellationToken);
        
    }
}

 


   