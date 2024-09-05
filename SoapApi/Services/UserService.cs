using System.ServiceModel;
using Microsoft.AspNetCore.Http.HttpResults;
using SoapApi.Contracts;
using SoapApi.Dtos;
using SoapApi.Mappers;
using SoapApi.Repositories;

namespace SoapApi.Services;

public class UserService : IUserContract{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository){
        _userRepository = userRepository;
        
    }

    public async Task<IList<UserResponseDto>> GetAll(CancellationToken cancellationToken){
        var user = await _userRepository.GetAllAsync(cancellationToken);
        if(user.Any()){
            return user.Select(user => user.ToDto()).ToList();
        }
        throw new FaultException("No users found");
        //Regresar todos los usuarios
    }
    public async Task<IList<UserResponseDto>> GetAllByEmail(string email, CancellationToken cancellationToken){
        var users = await _userRepository.GetByEmailAsync(email, cancellationToken);
            return users.Select(user => user.ToDto()).ToList();
        throw new FaultException("No users found with the provided email");
    }

    public async Task<UserResponseDto> GetUserById(Guid UserId, CancellationToken cancellationToken){
        var user = await _userRepository.GetByIdAsync(UserId, cancellationToken);
        if(user is not null){
            return user.ToDto();
        }
        throw new FaultException("User not found");
    }

    public async Task<bool> DeleteUserById(Guid UserId, CancellationToken cancellationToken){
        var user = await _userRepository.GetByIdAsync(UserId, cancellationToken);
        if(user is null){
            throw new FaultException("User not found");
        }
        await _userRepository.DeleteByIdAsync(user, cancellationToken);
        return true;
    }

    public async Task<UserResponseDto> CreateUser(UserCreateRequestDto userRequest, CancellationToken cancellationToken){
        var user = userRequest.ToModel();
        var createdUser= await _userRepository.CreateAsync(user, cancellationToken);
        return createdUser.ToDto();
    }

    public async Task<bool> UpdateUser(Guid id, string firstName, string lastName, DateTime birthday, CancellationToken cancellationToken)
        {
            return await _userRepository.UpdateAsync(id, firstName, lastName, birthday, cancellationToken);
        }


}