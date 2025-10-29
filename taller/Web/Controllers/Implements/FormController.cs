using Business.Interfaces.IBusinessImplements;
using Entity.Domain.Enums;
using Entity.DTOs.Default;
using Entity.DTOs.Select;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.ControllersBase.Web.Controllers.BaseController;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    [Produces("application/json")]
    public class FormController : BaseController<FormSelectDto,FormDto, IFormService>
    {
        public FormController(IFormService service, ILogger<FormController> logger) : base(service, logger)
        {
        }

        protected override Task<IEnumerable<FormSelectDto>> GetAllAsync(GetAllType g)
        {
            return _service.GetAllAsync(g);
        }
        protected override Task<FormSelectDto?> GetByIdAsync(int id)
        {
            return _service.GetByIdAsync(id);
        }
        protected override Task AddAsync(FormDto dto)
        {
            return _service.CreateAsync(dto);
        }

        protected override Task<bool> UpdateAsync(int id, FormDto dto)
        {
            return _service.UpdateAsync(dto);
        }

        protected override async Task<bool> DeleteAsync(int id, DeleteType deleteType)
        {
            var form = await _service.GetByIdAsync(id);
            if (form is null) return false;

            await _service.DeleteAsync(id, deleteType);
            return true;
        }

        protected override Task<bool> RestaureAsync(int id) => _service.RestoreLogical(id);
    }
}
