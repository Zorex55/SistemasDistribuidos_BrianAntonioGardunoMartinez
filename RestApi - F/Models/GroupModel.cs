namespace RestApi.Models;

public class GroupModel{

    public string Id { get; set; }

    public string Name { get; set; }

    public Guid[] Users { get; set; } //El arreglo va a guardar los id para que funcione bien con Mongo

    public DateTime CreationDate { get; set; }
}