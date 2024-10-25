using System.ServiceModel;
using RestApi.Infraestructure.Soap.SoapContracts;
using RestApi.Mappers;
using RestApi.Models;

namespace RestApi.Repositories;

public class UserRepository : IUserRepository
{

    //Inyección de un logger
    private readonly ILogger<UserRepository> _logger;

    private readonly IUserContract _userContract;

    public UserRepository(ILogger<UserRepository> logger, IConfiguration configuration){
        _logger = logger;
        var binding = new BasicHttpBinding();
        var endpoint = new EndpointAddress(configuration.GetValue<string>("UserServiceEndpoint"));
        _userContract = new ChannelFactory<IUserContract>(binding, endpoint).CreateChannel();
    }
    public async Task<UserModel> GetByIdAsync(Guid UserId, CancellationToken cancellationToken){
        try{
            var user = await _userContract.GetUserById(UserId, cancellationToken);
            return user.ToDomain();
            
        }catch(FaultException e) when (e.Message == "User not found"){
            _logger.LogWarning("User not found: {UserId}", UserId);
            return null;
        }
    }
}