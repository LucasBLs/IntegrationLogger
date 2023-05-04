using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IntegrationLogger.Utils;
public class RoleBasedAuthorizationFilter : IAuthorizationFilter
{
    private readonly string _role;

    public RoleBasedAuthorizationFilter(string role = null)
    {
        _role = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (_role != null)
        {
            if (user.Identity == null || !user.Identity.IsAuthenticated || !user.IsInRole(_role))
            {
                context.Result = new ChallengeResult();
            }
        }
    }
}