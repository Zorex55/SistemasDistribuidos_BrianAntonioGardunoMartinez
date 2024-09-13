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
}