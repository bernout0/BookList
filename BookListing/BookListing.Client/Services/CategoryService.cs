using BookListing.Client.Services.Interfaces;
using BookListing.Domain.Entities;
using System.Net.Http.Headers;

namespace BookListing.Client.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public CategoryService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<List<Category>> GetCategories()
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("Categories");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<Category>>();
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
