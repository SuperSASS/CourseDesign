using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CourseDesign.Shared.DTOs
{
    /// <summary>
    /// 充当应用层和数据库层的中介——数据传输层
    /// </summary>
    public class BaseDTO : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public DateTime CreateDate { get; set; }
        // public DateTime UpdateDate { get; set; }
        // 注：创建日期和修改日期在一层便可以忽略，应用端和服务端并不需要通讯此属性。TODO: 3 - 后面可以加上对修改时间的反馈

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 实现通知更新【虽然有点不明白为什么会在DTO层更新
        /// </summary>
        /// <param name="propertyName">要更新的属性名称</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
