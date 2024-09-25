using System.ServiceModel;
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
        if(users.Any()){
            return users.Select(user => user.ToDto()).ToList();
        }
        throw new FaultException("No users found with the provided email");
    }

    public async Task<UserResponseDto> GetUserById(Guid UserId, CancellationToken cancellationToken){
        var user = await _userRepository.GetByIdAsync(UserId, cancellationToken);
        if(user is not null){
            return user.ToDto();
        }
        throw new FaultException("User not found");
    }
}