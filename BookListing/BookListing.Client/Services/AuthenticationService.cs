namespace BookListing.Client.Services
{
	public class AuthenticationService
	{
		private readonly HttpClient _httpClient;

		public AuthenticationService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<string?> LoginAsync(string username, string password)
		{
			var response = await _httpClient.PostAsJsonAsync("Auth/login", new { Username = username, Password = password });

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<LoginResult>();
				return result?.Token;
			}
			return null;
		}

        public async Task<string?> RegisterAsync(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/register", new { Username = username, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResult>();
                return result?.Token;
            }
            return null;
        }
    }

	public class LoginResult
	{
		public string? Token { get; set; }
	}
}
