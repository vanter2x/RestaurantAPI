using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RegisterUserDtoValidator: AbstractValidator<RegisterUserDto>
    {
        private readonly RestaurantDbContext _context;
        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            _context = dbContext;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password).MinimumLength(6);

            RuleFor(x => x.ConfirmPassword).Equal(p => p.Password);

            RuleFor(x => x.Email).Custom((value, context) =>
            {
                var emailInUse = _context.Users.Any(u => u.Email == value);
                if (emailInUse)
                    context.AddFailure("Email", "Email already exist");
            });
        }

    }
}
