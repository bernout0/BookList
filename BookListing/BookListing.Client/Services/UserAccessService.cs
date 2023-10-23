using BookListing.Domain.Entities;
using System.Net.Http.Headers;

namespace BookListing.Client.Services
{
    public class UserAccessService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public UserAccessService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }


        public async Task CreateUserAccess(UserAccessRequest userAccessDto)
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync("UserAccess", userAccessDto);
        }

        public async Task DeleteUserAccess(Guid id)
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync("UserAccess/" + id);
        }
    }
}
