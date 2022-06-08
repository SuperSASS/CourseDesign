using CourseDesign.Common.Classes;
using CourseDesign.Services;
using CourseDesign.Services.Interfaces;
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
        public static int MaxTDoll_ID; // 最大的TDoll_ID

        public TDollsContext(ITDollService tDollService)
        {
            AllTDolls = new List<TDollClass>();
            TDollService = tDollService;
            FirstLoadTDollsAsync();
        }

        /// <summary>
        /// 读取所有数据库中的人形数据到本地
        /// </summary>
        async void FirstLoadTDollsAsync()
        {
            AllTDolls.Clear();
            MaxTDoll_ID = 0;

            var tDollResult = await TDollService.GetParamContain(new GETParameter()); // 读取所有人形
            if (tDollResult != null && tDollResult.Status == APIStatusCode.Success)
            {
                foreach (var item in tDollResult.Result.Items)
                {
                    AllTDolls.Add(new TDollClass(item.ID, item.Name, item.Rarity, (TDollType)(item.Type), item.ArtworkPath));
                    MaxTDoll_ID = MaxTDoll_ID < item.ID ? item.ID : MaxTDoll_ID;
                }
            }
        }

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
    }
}
