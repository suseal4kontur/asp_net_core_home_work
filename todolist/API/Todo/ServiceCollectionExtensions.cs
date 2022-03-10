namespace API.Todo
{
    using Microsoft.Extensions.DependencyInjection;

    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTodos(this IServiceCollection services)
        {
            services.AddSingleton<ITodoRepository, TodoRepository>();
            return services;
        }
    }
}
