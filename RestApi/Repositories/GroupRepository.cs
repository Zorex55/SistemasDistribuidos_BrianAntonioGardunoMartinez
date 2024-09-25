using MongoDB.Driver;
using RestApi.Infraestructure.Mongo;
using RestApi.Mappers;
using RestApi.Models;

namespace RestApi.Repositories;

public class GroupRepository : IGroupRepository{
    
    private readonly IMongoCollection<GroupEntity> _groups;

    public GroupRepository(IMongoClient mongoClient, IConfiguration configuration){ //Inyección de dependencias
        var database = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDb:Groups:DatabaseName"));
        _groups = database.GetCollection<GroupEntity>(configuration.GetValue<string>("MongoDb:Groups:CollectionName"));

    }
    
    public async Task<GroupModel> GetByIdAsync(string id, CancellationToken cancellationToken){
        try{
            var filter = Builders<GroupEntity>.Filter.Eq(x => x.Id, id); //Hacemos la query al estilo MongoDb para sacar el id por coincidencia
            var group = await _groups.Find(filter).FirstOrDefaultAsync(cancellationToken); //Que tome el primero que encuentre o lo hace nulo de lo contrario
            return group.ToModel();
        }catch(FormatException){
            return null;
        }
    }

    public async Task<List<GroupModel>> GetByNameAsync(string name, CancellationToken cancellationToken)
{
    // Usamos una expresión regular para hacer la búsqueda insensible a mayúsculas y minúsculas
    var filter = Builders<GroupEntity>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

    // Hacemos la búsqueda en MongoDB
    var groupEntities = await _groups.Find(filter).ToListAsync(cancellationToken);

    // Convertimos manualmente los resultados a `GroupModel`
    var groupModels = groupEntities.Select(g => new GroupModel
    {
        Id = g.Id,
        Name = g.Name,
        Users = g.Users,
        CreationDate = g.CreatedAt
    }).ToList();

    return groupModels;
}

}