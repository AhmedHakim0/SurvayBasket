using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SurvayBasket.API.Authentication;
using SurvayBasket.API.Presistance;

namespace SurvayBasket.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddAuthConfig(configuration);
        services.AddMapsterConfig()
                .AddFluentVlidationConfig();

        var ConnectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new Exception("Connection string 'DefaultConnection' not found");

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionString));


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
        services.AddScoped<IAuthService,AuthService>();
        services.AddScoped<IPollService, PollService>();


        return services;
    }
    public static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mappingconfig = TypeAdapterConfig.GlobalSettings;
        mappingconfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingconfig));
        return services;
    }


    public static IServiceCollection AddFluentVlidationConfig(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection AddAuthConfig(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddSingleton<IJwtProvidor,JwtProvidor>();
        // services.Configure<JwtOption>(configuration.GetSection(JwtOption.SectionName));
        services.AddOptions<JwtOption>()
             .BindConfiguration(JwtOption.SectionName)
             .ValidateDataAnnotations()
             .ValidateOnStart();


        var JwtSetting = configuration.GetSection(JwtOption.SectionName).Get<JwtOption>();
        services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting?.Key!)),
                    ValidIssuer = JwtSetting?.Issuer,
                    ValidAudience = JwtSetting?.Audience
                };
            });
        return services;
    }

}
