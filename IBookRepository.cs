using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SoapApi.Models;

namespace SoapApi.Repositories{
    public interface IBookRepository{

        public Task<BookModel> GetByIdAsync(Guid bookId, CancellationToken cancellationToken);

        public Task DeleteByIdAsync(BookModel book, CancellationToken cancellationToken);       
    }
}