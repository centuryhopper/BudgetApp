
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

    public string GetConfigurationValue(string configKey, string environmentVariableName)
    {
        return _env.IsDevelopment() 
            ? _configuration[configKey] 
            : Environment.GetEnvironmentVariable(environmentVariableName);
    }
}