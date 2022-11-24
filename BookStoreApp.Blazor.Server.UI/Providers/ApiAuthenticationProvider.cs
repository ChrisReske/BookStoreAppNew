using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookStoreApp.Blazor.Server.UI.Providers
{
    public class ApiAuthenticationProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

        public ApiAuthenticationProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage 
                            ?? throw new ArgumentNullException(nameof(localStorage));
            jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Retrieve token from local storage

            var user = new ClaimsPrincipal(new ClaimsIdentity());
            var savedToken = await _localStorage.GetItemAsync<string>("accessToken");
            if(savedToken == null)
            {
                return new AuthenticationState(user);
            }

            // Get the token content in strongly typed form
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(savedToken);
            if(tokenContent.ValidTo < DateTime.Now)
            {
                return new AuthenticationState(user);
            }

            var claims = tokenContent.Claims;

            user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            return new AuthenticationState(user);
        }
    }
}
