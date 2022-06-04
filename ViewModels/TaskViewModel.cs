using CourseDesign.Command.Classes;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.ViewModels
{
    internal class TaskViewModel : BindableBase
    {
        public DelegateCommand AddTaskCommand { get; private set; }
        private bool isRightDrawerOpen;
        private ObservableCollection<TasksBase> tasks;

        /// <summary>
        /// 右侧编辑窗是否弹出
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<TasksBase> Tasks
        {
            get { return tasks; }
            set { tasks = value; RaisePropertyChanged(); }
        }
        public TaskViewModel()
        {
            Tasks = new ObservableCollection<TasksBase>();
            AddTaskCommand = new DelegateCommand(Add);
            TEST_CreateTasks();
        }

        /// <summary>
        /// 增加计划
        /// </summary>
        private void Add()
        {
            IsRightDrawerOpen = true;
        }

        void TEST_CreateTasks()
        {
            int image_ID = 0, text_ID = 0;
            for (int i=1; i<=10; i++)
            {
                if (i % 2 == 0) // 用偶数来模拟是文字类任务 
                {
                    Tasks.Add(new TextTasksClass(++text_ID, false, "文字类计划测试", "内容这里是！……"));
                    Tasks.Add(new TextTasksClass(++text_ID, false, "文字类计划测试", "长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容"));
                }
                else
                    Tasks.Add(new ImageTasksClass(++image_ID, false, 1));
            }
        }
    }
}
