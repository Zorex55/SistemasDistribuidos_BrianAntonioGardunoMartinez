using SoapApi.Dtos;
using SoapApi.Infraestructure.Entities;
using SoapApi.Models;

namespace SoapApi.Mappers;

public static class BookMapper{

    public static BookModel ToModel(this BookEntity book){
        if (book is null){
            return null;
        }
        
        return new BookModel{
            Id = book.Id,
        };
    }

    public static BookEntity ToEntity(this BookModel book){
        return new BookEntity{
            Id = book.Id,
        };
    }
}