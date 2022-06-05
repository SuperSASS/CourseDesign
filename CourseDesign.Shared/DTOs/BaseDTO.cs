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
        public DateTime UpdateDate { get; set; }
        // 注：创建日期在一层便可以忽略，应用端并不需要在意此属性。

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 实现通知更新
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
