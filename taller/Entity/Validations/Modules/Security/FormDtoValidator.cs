using Entity.DTOs.Default;
using Entity.Validations.Modules.Generic;
using FluentValidation;

namespace Entity.Validations.Modules.Security
{
    public class FormDtoValidator : AbstractValidator<FormDto>
    {
        public FormDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name).NameRules();
            RuleFor(x => x.Description).DescriptionRules();

        }
    }
}
