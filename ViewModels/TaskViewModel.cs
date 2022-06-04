using CourseDesign.Command.Classes;
using CourseDesign.Services.Interfaces;
using CourseDesign.Shared.Parameters;
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
        private readonly IImageTaskService ImageService;

        /// <summary>
        /// 右侧编辑窗是否弹出
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 所有的任务列表
        /// </summary>
        public ObservableCollection<TasksBase> Tasks
        {
            get { return tasks; }
            set { tasks = value; RaisePropertyChanged(); }
        }
        public TaskViewModel(IImageTaskService imageService)
        {
            Tasks = new ObservableCollection<TasksBase>();
            AddTaskCommand = new DelegateCommand(Add);
            CreateTasks();
            ImageService = imageService;
        }

        /// <summary>
        /// 增加计划
        /// </summary>
        private void Add()
        {
            IsRightDrawerOpen = true;
        }

        async void CreateTasks()
        {
            // TODO: 警告，这里没有加外键约束，所有人都可以访问到所有Task
            var imageTaskResult = await ImageService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 100 }); // 通过服务，查询数据库ImageTask中所有元组，最多查询前100项（分页大小为100）

            if (imageTaskResult.Status)
            {
                foreach(var item in imageTaskResult.Result.Items)
                {
                    ImageTaskDTO
                }
            }

            // 以下为测试样例
            //int image_ID = 0, text_ID = 0;
            //for (int i=1; i<=10; i++)
            //{
            //    if (i % 2 == 0) // 用偶数来模拟是文字类任务 
            //    {
            //        Tasks.Add(new TextTasksClass(++text_ID, false, "文字类计划测试", "内容这里是！……"));
            //        Tasks.Add(new TextTasksClass(++text_ID, false, "文字类计划测试", "长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容"));
            //    }
            //    else
            //        Tasks.Add(new ImageTasksClass(++image_ID, false, 1));
            //}
        }
    }
}
