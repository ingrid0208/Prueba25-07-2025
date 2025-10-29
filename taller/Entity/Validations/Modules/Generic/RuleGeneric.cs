using FluentValidation;

namespace Entity.Validations.Modules.Generic
{
    public static class RuleGeneric
    {
        public static IRuleBuilderOptions<T, string> NameRules<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("El nombre es obligatorio.")
            .Length(5, 100).WithMessage("El nombre debe tener entre 5 y 100 caracteres.");

        public static IRuleBuilderOptions<T, string> DescriptionRules<T>(this IRuleBuilder<T, string> rule) =>
            rule.NotEmpty().WithMessage("La descripción es obligatoria.")
                .Length(10, 300).WithMessage("La descripción debe tener entre 10 y 300 caracteres.");

        public static IRuleBuilderOptions<T, int> IdRules<T>(this IRuleBuilder<T, int> rule) =>
            rule.GreaterThan(0).WithMessage("El ID debe ser un número positivo mayor que cero.");

    }
}
