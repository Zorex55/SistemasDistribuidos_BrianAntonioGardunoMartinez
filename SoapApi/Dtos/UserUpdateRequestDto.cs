using System.Runtime.Serialization;

namespace SoapApi.Dtos;
[DataContract]
public class UserUpdateRequestDto{
    
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public string FirstName {get; set; }

    [DataMember]
    public string LastName {get; set; } 

    [DataMember]
    public DateTime BirthDate {get; set; }

}