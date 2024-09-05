using Microsoft.EntityFrameworkCore;
using SoapApi.Infraestructure;
using SoapApi.Models;
using SoapApi.Mappers;

namespace SoapApi.Repositories;

public class UserRepository : IUserRepository
{

    private readonly RelationalDbContext _dbContext;

    public UserRepository(RelationalDbContext dbContext){
        _dbContext = dbContext;
    }

    
    public async Task<UserModel> CreateAsync(UserModel user, CancellationToken cancellationToken)
    {
        var userEntity = user.ToEntity();
        userEntity.Id = Guid.NewGuid();
        await _dbContext.AddAsync(userEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return userEntity.ToModel();
    }

    public async Task DeleteByIdAsync(UserModel user, CancellationToken cancellationToken)
    {
        var userEntity = user.ToEntity();
        _dbContext.Users.Remove(userEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IList<UserModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking().ToListAsync(cancellationToken);
        return user.Select(user => user.ToModel()).ToList();
    }

 public async Task<IList<UserModel>> GetByEmailAsync(string email, CancellationToken cancellationToken)
{
    // Búsqueda que retorna todos los correos que contengan la cadena proporcionada
    var user = await _dbContext.Users.AsNoTracking().Where(user => user.Email.Contains(email)).ToListAsync(cancellationToken);
    return user.Select(user => user.ToModel()).ToList();
}

    public async Task<UserModel> GetByIdAsync(Guid UserId, CancellationToken cancellationToken){
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == UserId, cancellationToken);
        return user.ToModel();
  }

    public async Task<bool> UpdateAsync(Guid id,string firstName,string lastName,DateTime birthday, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);

        if (user == null)
        {
            return false; // Usuario no encontrado
        }

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Birthday = birthday;

        _dbContext.Users.Update(user);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true; // Actualización exitosa
        }
        catch (Exception)
        {
            // Manejo de errores (opcional)
            return false; // Error en la actualización
        }
    }
}