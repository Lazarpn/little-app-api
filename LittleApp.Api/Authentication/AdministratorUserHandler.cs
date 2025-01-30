using LittleApp.Common.Enums;
using LittleApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LittleApp.Api.Authentication;

public class AdministratorUserHandler : AuthorizationHandler<AdministratorUserRequirement>
{
    private readonly LittleAppDbContext db;

    public AdministratorUserHandler(LittleAppDbContext db)
    {
        this.db = db;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdministratorUserRequirement requirement)
    {
        var isUserAdministrator = await CheckIfUserIsAdministrator(context);

        if (isUserAdministrator)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    public async Task<bool> CheckIfUserIsAdministrator(AuthorizationHandlerContext context)
    {
        if (!AuthorizationHelper.TryParseUserId(context, out var userId))
        {
            return false;
        }

        var userInfo = await db.Users
            .Where(u => u.Id == userId)
            .Select(u => new { Id = u.Id, EmailConfirmed = u.EmailConfirmed })
            .FirstOrDefaultAsync();

        if (userInfo == null || !userInfo.EmailConfirmed)
        {
            return false;
        }

        var isAdmin = await db.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.Role.Name == UserRoleConstants.Administrator);

        return isAdmin;
    }
}

public class AdministratorUserRequirement : IAuthorizationRequirement
{
}
