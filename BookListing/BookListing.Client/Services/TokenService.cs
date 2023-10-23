using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BookListing.Client.Services
{
    public class TokenService : ITokenService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _config;

        public TokenService(IJSRuntime jsRuntime, IConfiguration config)
        {
            _jsRuntime = jsRuntime;
            _config = config;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }

        public async Task SetTokenAsync(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        }

        public async Task RemoveTokenAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }

        /// <summary>
        /// Checks if user is admin via jwt 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> IsAdminBasedOnToken()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (token == null) return false;


            var secret = _config["secret"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            var validJwt = validatedToken as JwtSecurityToken;

            if (validJwt == null)
            {
                throw new ArgumentException("Invalid JWT");
            }

            Console.WriteLine("Token is valid. Decoded Claims:");
            foreach (var claim in principal.Claims)
            {
                if (claim.Value.Contains("admin@localhost")) return true; //TODO: WORK AROUND. ADD IN JWT IF IT'S ADMIN
            }

            return false;
        }
    }
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
        Task SetTokenAsync(string token);
        Task RemoveTokenAsync();
        Task<bool> IsAdminBasedOnToken();
    }


}
