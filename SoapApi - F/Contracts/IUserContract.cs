using System.ServiceModel;
using SoapApi.Dtos;
namespace SoapApi.Contracts;
[ServiceContract]

//Este archivo define qué metodos están disponibles para aquellos que usan el servicio, primero se deben dar a conocer aquí
public interface IUserContract{
    [OperationContract]
    public Task<UserResponseDto> GetUserById(Guid userId, CancellationToken cancellationToken);

    [OperationContract]
    public Task<IList<UserResponseDto>> GetAll(CancellationToken cancellationToken);
       
    [OperationContract]
    public Task<IList<UserResponseDto>> GetAllByEmail(string email, CancellationToken cancellationToken); 
}
