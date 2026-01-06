namespace ConqCTF.Application.Auth.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(v => v.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(v => v.Password)
                .MinimumLength(8);
        }
    }
}
