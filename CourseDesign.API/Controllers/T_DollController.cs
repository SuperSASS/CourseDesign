using CourseDesign.API.Services.Interfaces;
using CourseDesign.API.Services.Response;
using CourseDesign.Shared.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseDesign.API.Controllers
{
    /// <summary>
    /// 战术人形的控制器层
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TDollController : ControllerBase
    {
        private readonly ITDollService Service;
        public TDollController(ITDollService service) { Service = service; }

        /// <summary>
        /// 得到某一ID的战术人形元组。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpGet]
        public async Task<APIResponseInner> GetID([FromQuery] int id) => await Service.GetIDAsync(id);

        /// <summary>
        /// 得到满足<see cref="GETParameter"/>条件的战术人形元组。
        /// <para>条件为：单字段包含</para>
        /// </summary>
        /// <param name="parameter">所需满足的条件（参数）</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpGet]
        public async Task<APIResponseInner> GetParamContain([FromQuery] GETParameter parameter) => await Service.GetParamContainAsync(parameter);

        /// <summary>
        /// 得到满足<see cref="GETParameter"/>条件的战术人形元组。
        /// <para>条件为：单字段匹配</para>
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpGet]
        public async Task<APIResponseInner> GetParamEqual([FromQuery] GETParameter parameter) => await Service.GetParamEqualAsync(parameter);
    }
}
