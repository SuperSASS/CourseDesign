using CourseDesign.Command.Classes;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CourseDesign.Command.Classes.T_DollClass;

namespace CourseDesign.ViewModels
{
    internal class ListViewModel : BindableBase
    {
        private ObservableCollection<T_DollClass> t_dolls;

        public ObservableCollection<T_DollClass> T_Dolls
        {
            get { return t_dolls; }
            set { t_dolls = value; }
        }

        public ListViewModel()
        {
            T_Dolls = new ObservableCollection<T_DollClass>();
            TEST_CreateTDolls();
        }

        void TEST_CreateTDolls()
        {
            for (int i = 1; i <= 15; i++)
                T_Dolls.Add(new T_DollClass(i, "SuperSASS", i % 4 + 1, (typeEnum)(i % 6), @"/Assets/T-Dolls/T-Doll-1.png"));
        }
    }
}
