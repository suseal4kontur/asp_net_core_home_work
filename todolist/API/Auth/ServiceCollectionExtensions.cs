namespace API.Auth
{
    using Microsoft.Extensions.DependencyInjection;

    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddHostedService<SessionsActualizer>();

            return services;
        }
    }
}
