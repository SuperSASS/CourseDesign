using CourseDesign.Common.Classes;
using CourseDesign.Services;
using CourseDesign.Services.API.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.Parameters;
using CourseDesign.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static CourseDesign.Common.Classes.TDollClass;

namespace CourseDesign.Context
{
    /// <summary>
    /// 本地所存储的从服务器读取的所有人形，是整个app共享的上下文
    /// </summary>
    public class TDollsContext
    {
        private readonly ITDollService TDollService;
        public static List<TDollClass> AllTDolls; // 所有的战术人形上下文

        #region 初始化
        public TDollsContext(ITDollService tDollService)
        {
            TDollService = tDollService;
            AllTDolls = new List<TDollClass>();
            AddTDollsWaitTasks();
        }

        /// <summary>
        /// 增加生成人形数据上下文所需的等待任务
        /// </summary>
        private void AddTDollsWaitTasks()
        {
            ContextWaitTasks.WaitTasks.Add(Task.Run(FirstLoadTDollsContext));
        }

        /// <summary>
        /// 第一次加载所有数据库中的人形数据到本地
        /// </summary>
        private async Task FirstLoadTDollsContext()
        {
            try
            {
                var tDollResult = await TDollService.GetUserAndParamContain(new GETParameter()); // 读取所有人形
                if (tDollResult != null && tDollResult.Status == APIStatusCode.Success)
                    foreach (var item in tDollResult.Result.Items)
                    {
                        string[] methods = item.ObtainMethod.Split(new char[] { '/' }); // 处理ObtainMethod，按/分割
                        AllTDolls.Add(new TDollClass(item.ID, item.Name, item.Rarity, (TDollType)(item.Type), item.ArtworkPath, methods));
                    }
            }
            // 跟LoginUserContext一样的
            finally { }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 得到人形数据
        /// </summary>
        /// <param name="ID">人形ID</param>
        /// <returns><see cref="TDollClass"/>类型的人形数据</returns>
        public static TDollClass GetTDoll(int ID)
        {
            foreach (var item in AllTDolls)
                if (item.ID == ID)
                    return item;
            return null;
        }
        #endregion
    }
}
