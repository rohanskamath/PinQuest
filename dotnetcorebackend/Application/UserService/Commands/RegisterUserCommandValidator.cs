using FluentValidation;

namespace dotnetcorebackend.Application.UserService.Commands
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email is required!")
                .EmailAddress().WithMessage("Invalid Email format");

            RuleFor(f => f.FullName)
                .NotEmpty().WithMessage("Fullname is required!")
                .MinimumLength(3).WithMessage("Fullname must be more than 3 letters long!");

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be more than 3 characters long!");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password is required!")
                .MinimumLength(8).WithMessage("Password must be more than 8 characters long!")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Password must contain only alphanumeric characters!");
        }
    }
}
