using BookListing.Domain.Entities;
using BookListing.Domain.Identity;
using System.Net.Http.Headers;

namespace BookListing.Client.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public UserService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("Users");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                return result;
            }
            else
            {
                return null;
            }
        }
    }

    public class UserDTO
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public List<UserAccessDto> UserAccesses { get; set; }

    }

    public class UserAccessDto
    {
        public Guid Id { get; set; }
        public CategoryDTO? Category { get; set; } = null!;
        public DepartmentDTO? Department { get; set; } = null!;

    }

    public class UserAccessRequest
    {
        public Guid UserId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? DepartmentId { get; set; }

    }

    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class DepartmentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
