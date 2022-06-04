using CourseDesign.Command.Classes;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            TEST_CreateTDolls();
        }

        void TEST_CreateTDolls()
        {
            for (int i = 1; i <= 15; i++)
                TDolls.Add(new TDollClass(i, "SuperSASS", i % 4 + 1, (TypeEnum)(i % 6), @"/Assets/T-Dolls/T-Doll-1.png"));
        }
    }
}
