using System.Net;
using Domain.Identity;
using Persistence.Data;
using Application.Errors;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Messaging;
using Application.User.DTOS;

namespace Application.User.Register;

public class RegisterHandler : ICommandHandler<RegisterCommand, UserDTO>
{
    private readonly ApplicationDbContext context;
    private readonly IJwtGenerator jwtGenerator;
    private readonly UserManager<ApplicationUser> userManager;

    public RegisterHandler(ApplicationDbContext context, IJwtGenerator jwtGenerator, UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.jwtGenerator = jwtGenerator;
        this.userManager = userManager;
    }

    public async Task<UserDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });

        if (await context.Users.AnyAsync(u => u.UserName == request.Username, cancellationToken))
            throw new RestException(HttpStatusCode.BadRequest, new { Username = "Username already exists" });

        var user = new ApplicationUser
        {
            UserName = request.Username,
            DisplayName = request.DisplayName,
            Email = request.Email,
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
            return new UserDTO(DisplayName: user.DisplayName, Token: jwtGenerator.CreateToken(user), Username: user.UserName);

        throw new RestException(HttpStatusCode.BadRequest, new
        {
            result.Errors,
        });
    }
}
