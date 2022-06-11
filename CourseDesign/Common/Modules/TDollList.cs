using CourseDesign.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Common.Modules
{
    /// <summary>
    /// 在ListView中用于展示人形数据的模块（主要是要根据不同稀有度展示不同星星……
    /// </summary>
    public class TDollList
    {
        private TDollClass tDoll;
        private bool[] stars;
        private string typeIconPath;
        private string rarityBackgroundPath;
        private string nameBackgroundPath;

        /// <summary>
        /// 人形基本数据
        /// </summary>
        public TDollClass TDoll
        {
            get { return tDoll; }
            set { tDoll = value; }
        }
        /// <summary>
        /// 人形星级
        /// </summary>
        public bool[] Stars
        {
            get { return stars; }
            set { stars = value; }
        }
        /// <summary>
        /// 枪械种类图标路径
        /// </summary>
        public string TypeIconPath
        {
            get { return typeIconPath; }
            set { typeIconPath = value; }
        }
        /// <summary>
        /// 人形稀有度图片背景路径
        /// </summary>
        public string RarityBackgroundPath
        {
            get { return rarityBackgroundPath; }
            set { rarityBackgroundPath = value; }
        }
        /// <summary>
        /// 人形稀有度名字横条路径
        /// </summary>
        public string NameBackgroundPath
        {
            get { return nameBackgroundPath; }
            set { nameBackgroundPath = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tDoll">图鉴中该人形数据</param>
        public TDollList(TDollClass tDoll)
        {
            TDoll = tDoll;
            Stars = new bool[TDoll.Rarity];
            TypeIconPath = @"\Assets\Icons\" + TDoll.Type.ToString() + @"\" + TDoll.Rarity + ".png";
            RarityBackgroundPath = @"\Assets\Images\Rarity\ImageBackground\" + TDoll.Rarity.ToString() + ".png";
            NameBackgroundPath = @"\Assets\Images\Rarity\NameBanner\" + TDoll.Rarity.ToString() + ".png";
        }
    }
}
