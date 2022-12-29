using FluentValidation;

namespace Application.User.Login
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(query => query.Email).NotEmpty().EmailAddress();
            RuleFor(query => query.Password).NotEmpty();
        }
    }
}
