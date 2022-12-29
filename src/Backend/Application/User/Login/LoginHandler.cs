using System.Net;
using Domain.Identity;
using Application.Errors;
using Application.User.DTOS;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces.Messaging;

namespace Application.User.Login;

public class LoginHandler : IQueryHandler<LoginQuery, UserDTO>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IJwtGenerator jwtGenerator;

    public LoginHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtGenerator jwtGenerator)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.jwtGenerator = jwtGenerator;
    }

    public async Task<UserDTO> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new RestException(HttpStatusCode.Unauthorized);

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if(result.Succeeded)
        {
            return new(user.DisplayName, jwtGenerator.CreateToken(user), user.UserName);
        }

        throw new RestException(HttpStatusCode.Unauthorized);
    }
}
