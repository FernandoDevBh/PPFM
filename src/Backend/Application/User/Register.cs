using MediatR;
using System.Net;
using Domain.Identity;
using FluentValidation;
using Persistence.Data;
using Application.Errors;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Messaging;

namespace Application.User;

public class Register
{
    public record Command(string DisplayName, string Username, string Email, string Password) : ICommand<User> { }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class Handler : ICommandHandler<Command, User>
    {
        private readonly ApplicationDbContext context;        
        private readonly IJwtGenerator jwtGenerator;
        private readonly UserManager<ApplicationUser> userManager;

        public Handler(ApplicationDbContext context, IJwtGenerator jwtGenerator, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.jwtGenerator = jwtGenerator;
            this.userManager = userManager;
        }

        public async Task<User> Handle(Command request, CancellationToken cancellationToken)
        {            
            if(await context.Users.AnyAsync(u => u.Email == request.Email))
                throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });

            if(await context.Users.AnyAsync(u => u.UserName == request.Username))
                throw new RestException(HttpStatusCode.BadRequest, new { Username = "Username already exists" });

            var user = new ApplicationUser
            { 
                UserName = request.Username,
                DisplayName = request.DisplayName,
                Email = request.Email,
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if(result.Succeeded)            
                return new User(DisplayName: user.DisplayName, Token: jwtGenerator.CreateToken(user), Username: user.UserName);

            throw new RestException(HttpStatusCode.BadRequest, new
            {
                result.Errors,
            });
        }
    }
}
