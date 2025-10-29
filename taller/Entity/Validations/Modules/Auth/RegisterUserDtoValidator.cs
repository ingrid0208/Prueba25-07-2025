using Entity.DTOs.Auth;
using FluentValidation;

namespace Entity.Validations.Modules.Auth
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            // --- FirstName ---
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Length(2, 50).WithMessage("El nombre debe tener entre 2 y 50 caracteres.")
                .Matches(@"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$").WithMessage("El nombre solo puede contener letras y espacios.");

            // --- LastName ---
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .Length(2, 50).WithMessage("El apellido debe tener entre 2 y 50 caracteres.")
                .Matches(@"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$").WithMessage("El apellido solo puede contener letras y espacios.");

            // --- Address ---
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .Length(5, 150).WithMessage("La dirección debe tener entre 5 y 150 caracteres.");

            // --- Identification ---
            RuleFor(x => x.Identification)
                .NotEmpty().WithMessage("La identificación es obligatoria.")
                .Length(5, 20).WithMessage("La identificación debe tener entre 5 y 20 caracteres.")
                .Matches(@"^[A-Za-z0-9\-]+$").WithMessage("La identificación solo puede contener letras, números y guiones.");

            // --- PhoneNumber ---
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("El teléfono es obligatorio.")
                .Must(NotWhiteSpace).WithMessage("El teléfono no puede estar en blanco.")
                .Must(HasValidPhoneDigits).WithMessage("Debe tener entre 7 y 15 dígitos.")
                .MaximumLength(25).WithMessage("No debe superar 25 caracteres.");

            // --- Email ---
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
                .MaximumLength(100).WithMessage("El correo electrónico no puede superar los 100 caracteres.");

        }

        // Helpers
        private static bool NotWhiteSpace(string? s) =>
            !string.IsNullOrWhiteSpace(s);

        private static bool HasValidPhoneDigits(string? phone)
        {
           if (string.IsNullOrWhiteSpace(phone)) return false;
            int digits = 0;
            foreach (var ch in phone)
                if (char.IsDigit(ch)) digits++;
            return digits >= 7 && digits <= 15;
        }
    }
}
