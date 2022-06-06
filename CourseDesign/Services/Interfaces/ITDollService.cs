
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Services.Interfaces
{
    public interface ITDollService : IBaseService<TDollDTO>
    {
        public Task<APIResponse<PagedList<TDollDTO>>> GetParamContain(QueryParameter parameter);
        public Task<APIResponse<PagedList<TDollDTO>>> GetParamEqual(QueryParameter parameter);
        public Task<APIResponse<TDollDTO>> GetID(int id);
    }
}
