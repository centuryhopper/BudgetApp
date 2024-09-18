
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Contexts;
using Server.Entities;
using Shared.Models;
using static Shared.Models.ServiceResponses;

namespace Server.Repositories;

public class AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, BudgetDBContext budgetDBContext, ConfigurationProvider configurationProvider) : IAccountRepository
{
    public async Task<LoginResponse> LoginAccount(LoginDTO loginDTO)
    {
        if (loginDTO is null)
        {
            return new LoginResponse(false, null!, "Login container is empty");
        }

        var getUser = await userManager.FindByEmailAsync(loginDTO.Email);
        if (getUser is null)
        {
            return new LoginResponse(false, null!, "User not found");
        }

        bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, loginDTO.Password);
        if (!checkUserPasswords)
        {
            return new LoginResponse(false, null!, "Invalid email/password");
        }

        // your custom new user insert goes here (below is just an example)

        // add user to budget user table if they arent in it already
        if (await budgetDBContext.Users.FirstOrDefaultAsync(u => u.UmsUserid == getUser.Id) is null)
        {
            try
            {
                await budgetDBContext.Users.AddAsync(new User {
                    UmsUserid = getUser.Id!
                    ,Email = getUser.Email!,
                    Firstname = "test_firstname",
                    Lastname = "test_lastname"
                    ,Datecreated = DateTime.Now
                    ,Datelastlogin = DateTime.Now
                    ,Dateretired = null
                    ,Datelastlogout = null,
                });
                await budgetDBContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                return new LoginResponse(false, null!, ex.Message);
            }
        }

        var budgetUser = await budgetDBContext.Users.FirstOrDefaultAsync(u=>u.UmsUserid == getUser.Id);

        var getUserRole = await userManager.GetRolesAsync(getUser);
        string token = GenerateToken(budgetUser.Id, getUser.UserName, getUser.Email, getUserRole.First());

        return new LoginResponse(true, token!, "Login completed");
    }

    private string GenerateToken(int userId, string userName, string email, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configurationProvider.JwtKey
        ));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: configurationProvider.JwtIssuer,
            audience: configurationProvider.JwtAudience,
            claims: userClaims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}

