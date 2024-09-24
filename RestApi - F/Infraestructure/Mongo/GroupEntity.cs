using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RestApi.Infraestructure.Mongo;

public class GroupEntity{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] //Especifica la libería que en lugar de unar un object id, usa un string
    public string Id { get; set; }

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid[] Users { get; set; } 
}