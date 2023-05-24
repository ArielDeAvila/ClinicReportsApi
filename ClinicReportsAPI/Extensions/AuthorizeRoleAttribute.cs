using Microsoft.AspNetCore.Authorization;

namespace ClinicReportsAPI.Extensions;

public class AuthorizeRoleAttribute : AuthorizeAttribute
{
    public AuthorizeRoleAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}
