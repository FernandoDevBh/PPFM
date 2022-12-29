using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.User.Register;
using Application.User.DTOS;
using Application.User.Login;

namespace API.Controllers;

public class UserController : BaseController
{
    private readonly ISender sender;

    public UserController(ISender sender)
    {
        this.sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] LoginQuery query)
    {
        return await sender.Send(query);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterCommand command)
    {
        return await sender.Send(command);
    }
}
