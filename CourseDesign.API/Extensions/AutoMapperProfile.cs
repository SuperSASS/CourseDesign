using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.Shared.DTOs;

namespace CourseDesign.API.Extensions
{
    /// <summary>
    /// AutoMapper的配置
    /// </summary>
    public class AutoMapperProfile:MapperConfigurationExpression
    {
        public AutoMapperProfile()
        {
            CreateMap<ImagePlan, ImagePlanDTO>().ReverseMap(); // 使得ImagePlanDTO和dbImagePlan两者可以相互转换
            CreateMap<TextPlan, TextPlanDTO>().ReverseMap();   // 使得TextPlanDTO和dbTextPlan两者可以相互转换
            CreateMap<User, UserDTO>().ReverseMap();           // 使得UserDTO和dbUser两者可以相互转换
            CreateMap<TDoll, TDollDTO>().ReverseMap();         // 使得TDollDTO和dbTDoll两者可以相互转换
        }
    }
}
