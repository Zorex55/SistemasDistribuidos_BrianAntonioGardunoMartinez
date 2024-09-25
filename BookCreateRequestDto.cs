using System.Runtime.Serialization;

namespace SoapApi.Dtos;
[DataContract]
public class BookCreateRequestDto{
    
    [DataMember]
    public Guid Id { get; set; }

}