using Entity.DTOs.Default;
using Entity.Validations.Modules.Generic;
using FluentValidation;

namespace Entity.Validations.Modules.Security
{
    public class RolUserDtoValidator : AbstractValidator<RolUserDto>
    {
        public RolUserDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.RolId).IdRules();
            RuleFor(x => x.UserId).IdRules();
        }
    }
}
