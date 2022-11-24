using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(
        IClient httpClient, 
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient 
                      ?? throw new ArgumentNullException(nameof(httpClient));
        _localStorage = localStorage 
                        ?? throw new ArgumentNullException(nameof(localStorage));
    }

    public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
    {
        var response = await _httpClient.LoginAsync(loginModel);

        // STORE TOKEN IN LOCAL STORAGE
        await _localStorage.SetItemAsync("accessToken", response.Token);

        // CHANGE AUTH STATE


        return true;
    }
}