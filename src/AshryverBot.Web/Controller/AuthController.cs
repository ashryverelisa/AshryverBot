using AspNet.Security.OAuth.Twitch;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AshryverBot.Web.Controller;

[ApiController]
[Route("/")]
public class AuthController : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? returnUrl)
    {
        var safeReturn = IsSafeReturnUrl(returnUrl) ? returnUrl! : "/";
        return Challenge(
            new AuthenticationProperties { RedirectUri = safeReturn },
            TwitchAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromQuery] string? returnUrl)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var safeReturn = IsSafeReturnUrl(returnUrl) ? returnUrl! : "/";
        return LocalRedirect(safeReturn);
    }

    private static bool IsSafeReturnUrl(string? returnUrl) =>
        !string.IsNullOrEmpty(returnUrl)
        && returnUrl.StartsWith('/')
        && !returnUrl.StartsWith("//")
        && !returnUrl.StartsWith("/\\");
}
