using LittleApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LittleApp.Api.Authentication;

public class NotConfirmedEmailHandler : AuthorizationHandler<NotConfirmedEmailRequirement>
{
    private readonly LittleAppDbContext db;

    public NotConfirmedEmailHandler(LittleAppDbContext db)
    {
        this.db = db;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, NotConfirmedEmailRequirement requirement)
    {
        var emailNotConfirmed = await CheckEmailNotConfirmed(context);

        if (emailNotConfirmed)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    public async Task<bool> CheckEmailNotConfirmed(AuthorizationHandlerContext context)
    {
        if (!AuthorizationHelper.TryParseUserId(context, out var userId))
        {
            return false;
        }

        var userInfo = await db.Users
            .Where(u => u.Id == userId)
            .Select(u => new { Id = u.Id, EmailConfirmed = u.EmailConfirmed })
            .FirstOrDefaultAsync();

        return userInfo != null && !userInfo.EmailConfirmed;
    }
}

public class NotConfirmedEmailRequirement : IAuthorizationRequirement
{
}
