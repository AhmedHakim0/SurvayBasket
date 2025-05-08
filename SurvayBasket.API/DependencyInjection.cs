namespace SurvayBasket.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMapsterConfig()
                .AddFluentVlidationConfig();
            


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
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
 
}
