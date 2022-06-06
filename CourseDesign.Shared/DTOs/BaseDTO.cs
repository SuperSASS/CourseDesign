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
        public DateTime UpdateDate { get; set; }

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
