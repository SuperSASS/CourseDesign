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
            CreateMap<ImageTask, ImageTaskDTO>().ReverseMap(); // 使得ImageTaskDTO和dbImageTask两者可以相互转换
            CreateMap<TextTask, TextTaskDTO>().ReverseMap();   // 使得TextTaskDTO和dbTextTask两者可以相互转换
            CreateMap<User, UserDTO>().ReverseMap();           // 使得UserDTO和dbUser两者可以相互转换
            CreateMap<TDoll, TDollDTO>().ReverseMap();         // 使得TDollDTO和dbTDoll两者可以相互转换
        }
    }
}
