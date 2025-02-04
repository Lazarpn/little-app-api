using LittleApp.Api.Authentication;
using LittleApp.Api.Controllers;
using LittleApp.Common.Helpers;
using LittleApp.Common.Models.User;
using LittleApp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace LittleApp.Api.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : BaseController
{
    private readonly AccountManager accountManager;
    private readonly JwtHelper jwtHelper;

    public AccountController(AccountManager accountManager, IConfiguration configuration)
    {
        this.accountManager = accountManager;
        jwtHelper = new JwtHelper(configuration);
    }

    /// <summary>
    /// Registers and logins a new user using passed credentials
    /// </summary>
    /// <param name="model">User registration info</param>
    /// <returns>Login information</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseModel))]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
    {
        var authResponse = await accountManager.Register(model);
        return Ok(authResponse);
    }

    /// <summary>
    /// User login
    /// </summary>
    /// <param name="model">User's login credentials</param>
    /// <returns>Login information</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseModel))]
    public async Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        var authResponse = await accountManager.Login(model);
        return Ok(authResponse);
    }

    /// <summary>
    /// User login
    /// </summary>
    /// <returns>Login information</returns>
    [HttpGet("authenticated")]
    [Authorize(Policy = Policies.RegisteredUser)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Authenticated()
    {
        return NoContent();
    }
}
