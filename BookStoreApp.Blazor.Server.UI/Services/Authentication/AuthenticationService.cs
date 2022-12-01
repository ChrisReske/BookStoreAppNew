using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Providers;
using BookStoreApp.Blazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Server.UI.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationService(
        IClient httpClient, 
        ILocalStorageService localStorage, 
        AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient 
                      ?? throw new ArgumentNullException(nameof(httpClient));
        _localStorage = localStorage 
                        ?? throw new ArgumentNullException(nameof(localStorage));
        _authenticationStateProvider = authenticationStateProvider 
                                       ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
    }

    public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
    {
        var response = await _httpClient.LoginAsync(loginModel);

        // STORE TOKEN IN LOCAL STORAGE
        await _localStorage.SetItemAsync("accessToken", response.Token);

        // CHANGE AUTH STATE
        await ((ApiAuthenticationProvider)_authenticationStateProvider).LoggedIn();  

        return true;
    }
}