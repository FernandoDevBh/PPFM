using Constants;
using API.Config;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Extensions;

public static class JwtSecurityServices
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[Api.TOKEN_KEY] ?? string.Empty));
        var apiSettingsSection = configuration.GetSection("APISettings");
        services.Configure<ApiSettings>(apiSettingsSection);

        var apiSettings = apiSettingsSection.Get<ApiSettings>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(builder =>
        {
            builder.RequireHttpsMetadata = false;
            builder.SaveToken = true;
            builder.TokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = apiSettings?.ValidAudience,
                ValidIssuer = apiSettings?.ValidIssuer,
            };
        });
        return services;
    }
}
