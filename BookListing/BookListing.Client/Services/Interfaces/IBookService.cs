using BookListing.Domain.Entities;
using System;

namespace BookListing.Client.Services.Interfaces
{
    public interface IBookService
    {
        Task<List<Book>> GetBooks();
        Task<Book> GetBook(Guid id);

        Task DeleteBook(Book book);

        Task<Book> AddBook(Book book);

        Task UpdateBook(Book book);
    }
}
