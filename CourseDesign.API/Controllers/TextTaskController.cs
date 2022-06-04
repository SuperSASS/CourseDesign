using CourseDesign.API.Services;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseDesign.API.Controllers
{
    /// <summary>
    /// TextTask（计划列表的文字类计划）的控制器层
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TextTaskController : ControllerBase
    {
        private readonly ITextTaskService service;
        public TextTaskController(ITextTaskService service) { this.service = service; }

        [HttpGet]
        public async Task<APIResponse> Get(int id) => await service.GetSingleAsync(id);
        [HttpGet]
        public async Task<APIResponse> GetAll([FromQuery] QueryParameter parameter) => await service.GetAllAsync(parameter);
        [HttpPost]
        public async Task<APIResponse> Add([FromBody] TextTaskDTO model) => await service.AddAsync(model);
        [HttpPost]
        public async Task<APIResponse> Update([FromBody] TextTaskDTO model) => await service.UpdateAsync(model);
        [HttpDelete]
        public async Task<APIResponse> Delete(int id) => await service.DeleteAsync(id);
    }
}
