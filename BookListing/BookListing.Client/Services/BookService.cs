using BookListing.Client.Pages;
using BookListing.Client.Services.Interfaces;
using BookListing.Domain.Entities;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace BookListing.Client.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public BookService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<Book> AddBook(Book book)
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync("Books", book);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Book>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task DeleteBook(Book book)
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync("Books/" + book.Id);
        }

        public async Task<Book> GetBook(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Book>> GetBooks()
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("Books");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<Book>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateBook(Book book)
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsJsonAsync("Books/"+book.Id, book);

        }
    }
}
