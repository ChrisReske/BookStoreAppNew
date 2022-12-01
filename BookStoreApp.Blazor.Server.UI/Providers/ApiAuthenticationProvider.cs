using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Static;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookStoreApp.Blazor.Server.UI.Providers
{
    public class ApiAuthenticationProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public ApiAuthenticationProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage 
                            ?? throw new ArgumentNullException(nameof(localStorage));
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Retrieve token from local storage

            var user = new ClaimsPrincipal(new ClaimsIdentity());
            var savedToken = await _localStorage
                .GetItemAsync<string>(AuthenticationStrings.AccessToken);
            if(savedToken == null)
            {
                return new AuthenticationState(user);
            }

            // Get the token content in strongly typed form
            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
            if(tokenContent.ValidTo < DateTime.Now)
            {
                return new AuthenticationState(user);
            }

            var claims = tokenContent.Claims;

            user = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationStrings.AuthenticationType));

            return new AuthenticationState(user);
        }

        public async Task LoggedIn()
        {
            var savedToken = await _localStorage.GetItemAsync<string>(AuthenticationStrings.AccessToken);
            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
            var claims = tokenContent.Claims;
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationStrings.AuthenticationType));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void LoggedOut()
        {
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
