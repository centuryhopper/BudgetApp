
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

public class AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : IAccountRepository
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

        // add user to stock user table if they arent in it already
        // if (await stockDataDbContext.Stockusers.FirstOrDefaultAsync(u => u.UmsUserid == getUser.Id) is null)
        // {
        //     try
        //     {
                // await stockDataDbContext.Stockusers.AddAsync(new Stockuser {
                //     UmsUserid = getUser.Id!
                //     ,Email = getUser.Email!
                //     ,DateCreated = DateTime.Now
                //     ,DateLastLogin = DateTime.Now
                //     ,DateRetired = null
                // });
                // await stockDataDbContext.SaveChangesAsync();
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return new LoginResponse(false, null!, ex.Message);
        //     }
        // }

        // var stockUser = await stockDataDbContext.Stockusers.FirstOrDefaultAsync(u=>u.UmsUserid == getUser.Id);

        var getUserRole = await userManager.GetRolesAsync(getUser);
        string token = GenerateToken(0/*just a dummy id value for demo*/, getUser.UserName, getUser.Email, getUserRole.First());

        return new LoginResponse(true, token!, "Login completed");
    }

    private string GenerateToken(int userId, string userName, string email, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webHostEnvironment.IsDevelopment() ? configuration["Jwt:Key"] : Environment.GetEnvironmentVariable("JWT_Key")));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: webHostEnvironment.IsDevelopment() ? configuration["Jwt:Issuer"] : Environment.GetEnvironmentVariable("Jwt_Issuer"),
            audience: webHostEnvironment.IsDevelopment() ? configuration["Jwt:Audience"] : Environment.GetEnvironmentVariable("Jwt_Audience"),
            claims: userClaims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}

