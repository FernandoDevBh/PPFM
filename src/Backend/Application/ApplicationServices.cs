using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Application.Behaviors;
using Application.User.DTOS;

namespace Application;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(UserDTO).Assembly);
        services.AddValidatorsFromAssembly(typeof(ApplicationServices).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}
