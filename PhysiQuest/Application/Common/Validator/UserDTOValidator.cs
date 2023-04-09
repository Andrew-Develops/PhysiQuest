using Application.Users.DTO;
using FluentValidation;

namespace Application.Common.Validator
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            //RuleFor(user => user.Name)
            //    .NotEmpty().WithMessage("Name is required.")
            //    .Length(3, 50).WithMessage("Name must be between 3 and 50 characters.");
        }
    }
}
