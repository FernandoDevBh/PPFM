using Application.Interfaces.Messaging;
using Application.User.DTOS;

namespace Application.User.Register
{
    public record RegisterCommand(string DisplayName, string Username, string Email, string Password) : ICommand<UserDTO> { }
}
