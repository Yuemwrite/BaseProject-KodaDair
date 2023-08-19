using System.Security.Claims;

namespace Shared.Authorization;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.NameIdentifier);
    
    private static string? FindFirstValue(this ClaimsPrincipal principal, string claimType) =>
        principal is null
            ? throw new ArgumentNullException(nameof(principal))
            : principal.FindFirst(claimType)?.Value;
}