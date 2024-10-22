using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using TipsRundan.Application.Interfaces;
using TipsRundan.Domain.Entities;
using TipsRundan.Domain.Interfaces;

namespace TipsRundan.Infrastructure.Security;

public class CookieAuthnService : IAuthnService
{
    private readonly ICryptographyService _cryptography;
    private readonly IAccountRepository _userRepository;
    private readonly IHttpContextAccessor _httpContext;
    public CookieAuthnService(
        ICryptographyService cryptography,
        IAccountRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _cryptography = cryptography;
        _userRepository = userRepository;
        _httpContext = httpContextAccessor;
    }
    public void SigninUser(string userName, string passwordString)
    {
        // Validate signin credentials
        var user = _userRepository.ReadByName(userName);
        if (!_cryptography.Validate(passwordString, user.PasswordHash))
            throw new Exception("Password and username doesn't match.");

        // Construct cookie content
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        var principal = new ClaimsPrincipal(identity);

        // Sign in with http context
        _httpContext.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal
        );
    }

    public void SignoutUser()
    {
        _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
    public User GetCurrentUser()
    {
        var username = _httpContext.HttpContext.User.Identity.Name;
        return _userRepository.ReadByName(username);
    }
}