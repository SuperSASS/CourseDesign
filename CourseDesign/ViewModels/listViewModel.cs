using CourseDesign.Command.Classes;
using CourseDesign.Context;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using static CourseDesign.Command.Classes.TDollClass;

namespace CourseDesign.ViewModels
{
    internal class ListViewModel : BindableBase
    {
        private ObservableCollection<TDollClass> tdolls;

        public ObservableCollection<TDollClass> TDolls
        {
            get { return tdolls; }
            set { tdolls = value; }
        }

        public ListViewModel()
        {
            TDolls = new ObservableCollection<TDollClass>();
            foreach (var item in TDollsContext.tDolls) // 把API读取的加进来
                tdolls.Add(item);
        }

    }
}
