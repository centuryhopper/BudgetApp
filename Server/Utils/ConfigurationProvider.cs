
namespace Server.Utils;

public class ConfigurationProvider
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;

    public ConfigurationProvider(IWebHostEnvironment env, IConfiguration configuration)
    {
        _env = env;
        _configuration = configuration;
    }

    public string GetUserManagementConnectionString {
        get => _env.IsDevelopment() 
            ? _configuration.GetConnectionString("UserManagementDB")
            : Environment.GetEnvironmentVariable("UserManagementDB"); 
    }

    public string GetBudgetDBConnectionString {
        get => _env.IsDevelopment() 
            ? _configuration.GetConnectionString("BudgetDB")
            : Environment.GetEnvironmentVariable("BudgetDB"); 
    }

    public string JwtKey {get => _env.IsDevelopment() 
            ? _configuration["Jwt:Key"] 
            : Environment.GetEnvironmentVariable("Jwt_Key");}
    public string JwtIssuer {get => _env.IsDevelopment() 
            ? _configuration["Jwt:Issuer"] 
            : Environment.GetEnvironmentVariable("Jwt_Issuer");}
    public string JwtAudience {get => _env.IsDevelopment() 
            ? _configuration["Jwt:Audience"] 
            : Environment.GetEnvironmentVariable("Jwt_Audience");}

    public string GetConfigurationValue(string configKey, string environmentVariableName)
    {
        return _env.IsDevelopment() 
            ? _configuration[configKey] 
            : Environment.GetEnvironmentVariable(environmentVariableName);
    }
}