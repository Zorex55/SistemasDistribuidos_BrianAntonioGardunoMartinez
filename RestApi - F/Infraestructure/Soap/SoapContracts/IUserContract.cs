using System.ServiceModel;
namespace RestApi.Infraestructure.Soap.SoapContracts;
[ServiceContract]

//Este archivo define qué metodos están disponibles para aquellos que usan el servicio, primero se deben dar a conocer aquí
public interface IUserContract{
    [OperationContract]
    public Task<UserResponseDto> GetUserById(Guid userId, CancellationToken cancellationToken);

    [OperationContract]
    public Task<IList<UserResponseDto>> GetAll(CancellationToken cancellationToken);
       
    [OperationContract]
    public Task<IList<UserResponseDto>> GetAllByEmail(string email, CancellationToken cancellationToken); 

    [OperationContract]
    public Task<bool> DeleteUserById(Guid userId, CancellationToken cancellationToken);

    [OperationContract]
    public Task<UserResponseDto> CreateUser(CancellationToken cancellationToken);

    [OperationContract]
    public Task<bool> UpdateUser(Guid userId, string firstName, string lastName, DateTime birthday, CancellationToken cancellationToken);
}

