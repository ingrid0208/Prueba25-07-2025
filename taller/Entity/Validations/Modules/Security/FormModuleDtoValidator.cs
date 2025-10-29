using Entity.DTOs.Default;
using Entity.Validations.Modules.Generic;
using FluentValidation;

namespace Entity.Validations.Modules.Security
{
    public class FormModuleDtoValidator : AbstractValidator<FormModuleDto>
    {
        public FormModuleDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.FormId).IdRules();
            RuleFor(x => x.ModuleId).IdRules();


        }
    }
}
