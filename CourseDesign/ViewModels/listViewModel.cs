using CourseDesign.Common.Classes;
using CourseDesign.Common.Modules;
using CourseDesign.Context;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using static CourseDesign.Common.Classes.TDollClass;

namespace CourseDesign.ViewModels
{
    public class ListViewModel : BindableBase
    {
        private ObservableCollection<TDollList> tDollList;

        public ObservableCollection<TDollList> TDollList
        {
            get { return tDollList; }
            set { tDollList = value; }
        }

        public ListViewModel()
        {
            TDollList = new ObservableCollection<TDollList>();
            foreach (var item in TDollsContext.AllTDolls) // 把API读取的加进来
                TDollList.Add(new TDollList(item));
        }
    }
}
