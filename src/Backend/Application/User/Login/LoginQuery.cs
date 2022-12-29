using Application.User.DTOS;
using Application.Interfaces.Messaging;

namespace Application.User.Login
{
    public record LoginQuery(string Email, string Password) : IQuery<UserDTO> { }
}
