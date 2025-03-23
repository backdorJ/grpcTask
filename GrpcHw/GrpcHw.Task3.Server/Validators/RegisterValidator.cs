

using FluentValidation;

namespace GrpcHw.Task3.Server.Validators;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("Email is required");
        
        RuleFor(request => request.Password)
            .Matches(x => x.ConfirmPassword)
            .WithMessage("Password and confirmation password do not match");
    }
}