using BookListing.Client.Services.Interfaces;
using BookListing.Domain.Entities;
using System.Net.Http.Headers;

namespace BookListing.Client.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public DepartmentService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<List<Department>> GetDepartments()
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("Departments");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<Department>>();
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
