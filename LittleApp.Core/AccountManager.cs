using AutoMapper;
using LittleApp.Common.Enums;
using LittleApp.Common.Exceptions;
using LittleApp.Common.Helpers;
using LittleApp.Common.Models.User;
using LittleApp.Data;
using LittleApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Core;

public class AccountManager
{
    private readonly UserManager<User> userManager;
    private readonly IConfiguration configuration;
    private readonly IMapper mapper;
    private readonly JwtHelper jwtHelper;
    private readonly LittleAppDbContext db;
    private readonly int MINUTES_VERIFICATION_CODE_IS_VALID;

    public AccountManager(
        LittleAppDbContext db,
        UserManager<User> userManager,
        IConfiguration configuration,
        IMapper mapper)
    {
        this.db = db;
        this.userManager = userManager;
        this.configuration = configuration;
        this.mapper = mapper;
        jwtHelper = new JwtHelper(configuration);

        MINUTES_VERIFICATION_CODE_IS_VALID = Convert.ToInt32(configuration["minutesVerificationCodeIsValid"]);
    }


    public async Task<AuthResponseModel> Login(UserLoginModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        ValidationHelper.MustExist(user);

        bool isValidPassword = await userManager.CheckPasswordAsync(user, model.Password);

        if (!isValidPassword)
        {
            throw new ValidationException(ErrorCode.InvalidCredentials);
        }

        var loginResult = await GenerateLoginResponse(user);
        return loginResult;
    }

    public async Task<AuthResponseModel> Register(UserRegisterModel model)
    {
        var existingUser = await userManager.FindByEmailAsync(model.Email);
        ValidationHelper.MustNotExist(existingUser);

        var passwordValidator = new PasswordValidator<User>();
        var passwordValidationResult = await passwordValidator.ValidateAsync(userManager, null, model.Password);

        if (!passwordValidationResult.Succeeded)
        {
            throw new ValidationException(ErrorCode.WrongPassword);
        }

        var newUser = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Email,
            EmailConfirmed = true,
            Active = true
        };

        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            IdentityResult identityResult;

            // User registers with password
            if (string.IsNullOrEmpty(model.Password))
            {
                ValidationHelper.ThrowModelValidationException("Password", "Password is required.");
            }

            identityResult = await userManager.CreateAsync(newUser, model.Password);

            if (!identityResult.Succeeded)
            {
                throw new ValidationException(ErrorCode.IdentityError);
            }

            identityResult = await userManager.AddToRoleAsync(newUser, UserRoleConstants.Administrator);

            if (!identityResult.Succeeded)
            {
                throw new ValidationException(ErrorCode.IdentityError);
            }

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new ValidationException(ErrorCode.IdentityError);
        }

        var roles = await userManager.GetRolesAsync(newUser);
        var token = jwtHelper.GenerateJwtToken(newUser.Id, newUser.Email, roles);

        return new AuthResponseModel
        {
            Token = token,
            Roles = roles.ToList()
        };
    }

    private async Task<AuthResponseModel> GenerateLoginResponse(User user)
    {
        var roles = await userManager.GetRolesAsync(user);
        if (!roles.Any())
        {
            throw new ValidationException(ErrorCode.NoRoleAssigned);
        }

        var token = jwtHelper.GenerateJwtToken(user.Id, user.Email, roles);
        return new AuthResponseModel
        {
            Token = token,
            Roles = roles.ToList()
        };
    }
}
