using Entity.DTOs.Default;
using Entity.Validations.Modules.Generic;
using FluentValidation;

namespace Entity.Validations.Modules.Security
{
    public class RolFormPermissionDtoValidator : AbstractValidator<RolFormPermissionDto>
    {
        public RolFormPermissionDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.RolId).IdRules();
            RuleFor(x => x.FormId).IdRules();
            RuleFor(x => x.PermissionId).IdRules();



        }
    }
}
