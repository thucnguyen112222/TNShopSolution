using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNShopSolution.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required")
                .MaximumLength(200).WithMessage("First name canot over 200 characters");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(200).WithMessage("Last name canot over 200 characters");
            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100))
                .WithMessage("Birthday cannot greater then 100 year");
            RuleFor(x => x.Email).NotEmpty().WithMessage("email is required").Matches(@"^[^\s@]+@[^\s@]+\.[^\s@]+$")
                .WithMessage("email format not match");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password is at least 6 characters");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
            RuleFor(x => x).Custom((request, text) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    text.AddFailure("Confirm Password is not match");
                }
            });
        }
    }
}
