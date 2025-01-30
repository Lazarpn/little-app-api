using LittleApp.Api.Authentication;
using LittleApp.Common.Enums;
using LittleApp.Core;
using LittleApp.Data;
using LittleApp.Entities;
using LittleApp.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LittleApp.Api.Controllers;

[Route("api/utilities")]
[ApiController]
public class UtilityController : BaseController
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;
    private readonly LittleAppDbContext db;

    public UtilityController(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        LittleAppDbContext db)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.db = db;
    }

    [HttpPost]
    [Route("seed-data")]
    [Authorize(Policy = Policies.AdministratorUser)]
    public async Task<IActionResult> SeedData()
    {
        await db.SeedData(userManager, roleManager);
        return NoContent();
    }
}
