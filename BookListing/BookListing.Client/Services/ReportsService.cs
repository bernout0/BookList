using System.Net.Http.Headers;

namespace BookListing.Client.Services
{
    public class ReportsService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public ReportsService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }


        public async Task<TotalBookCountDto> GetTotalBookCount()
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("Reports/total-books");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TotalBookCountDto>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<AuthorBookCountDto>> GetBooksPerAuthor()
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("Reports/books-per-author");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<AuthorBookCountDto>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<CategoryBookCountDto>> GetBooksPerCategory()
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("Reports/books-per-category");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<CategoryBookCountDto>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<DepartmentBookCountDto>> GetBooksPerDepartment()
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("Reports/books-per-department");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<DepartmentBookCountDto>>();
                return result;
            }
            else
            {
                return null;
            }
        }






        public class TotalBookCountDto
        {
            public int BookCount { get; set; }
        }


        public class AuthorBookCountDto
        {
            public string Author { get; set; }
            public int BookCount { get; set; }
        }

        public class CategoryBookCountDto
        {
            public string CategoryName { get; set; }
            public int BookCount { get; set; }
        }

        public class DepartmentBookCountDto
        {
            public string DepartmentName { get; set; }
            public int BookCount { get; set; }
        }
    }
}
