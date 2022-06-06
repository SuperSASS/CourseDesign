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
    public class TDollsContext
    {
        private readonly ITDollService TDollService;
        public static List<TDollClass> tDolls;

        public TDollsContext(ITDollService tDollService)
        {
            tDolls = new List<TDollClass>();
            TDollService = tDollService;
            FirstLoadTDollsAsync();
        }

        /// <summary>
        /// 读取所有数据库中的人形数据
        /// </summary>
        async void FirstLoadTDollsAsync()
        {
            tDolls.Clear();
            var tDollResult = await TDollService.GetParamContain(new QueryParameter());

            if (tDollResult != null && tDollResult.Status == APIStatusCode.Success)
            {
                foreach (var item in tDollResult.Result.Items)
                    tDolls.Add(new TDollClass(item.ID, item.Name, item.Rarity, (TypeEnum)(item.Type), item.ArtworkPath));
            }
        }

        public static TDollClass GetTDoll(int ID)
        {
            foreach (var item in tDolls)
                if (item.ID == ID)
                    return item;
            return null;
        }
    }
}
