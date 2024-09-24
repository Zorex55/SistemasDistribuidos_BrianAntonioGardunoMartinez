using RestApi.Dtos;
using RestApi.Infraestructure.Mongo;
using RestApi.Models;

namespace RestApi.Mappers;

public static class GroupMapper{
    
    public static GroupResponse ToDto(this GroupUserModel group){
        return new GroupResponse{
            Id = group.Id,
            Name = group.Name,
            CreationTime = group.CreationTime
        };
    }

    public static GroupModel ToModel(this GroupEntity group){
        if (group is null){
            return null;
        }

        return new GroupModel{
            Id = group.Id,
            Name = group.Name,
            Users = group.Users,
            CreationDate = group.CreatedAt
        };
    }
} 