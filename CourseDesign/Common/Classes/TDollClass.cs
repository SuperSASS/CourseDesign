using System.Collections.Generic;

namespace CourseDesign.Common.Classes
{
    /// <summary>
    /// 战术人形类基本信息
    /// <para>从数据库中导入</para>
    /// </summary>
    /// <list type="number">
    /// <item><c>ID</c> - 人形编号</item>
    /// <item><c>Name</c> - 人形名称</item>
    /// <item><c>Rarity</c> - 人形稀有度</item>
    /// <item><c>Type</c> - 人形种类</item>
    /// <item><c>Artwork</c> - 人形立绘</item>
    /// </list>
    public class TDollClass
    {
        public enum TDollType { HG, SMG, RF, AR, MG, SG }; // 计划类型的枚举
        #region 字段
        private int id;
        private string name;
        private int rarity;
        private TDollType type;
        private string artworkPath;
        private string[] obtainMethod;
        #endregion

        #region 属性
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Rarity
        {
            get { return rarity; }
            set { rarity = value; }
        }
        public TDollType Type
        {
            get { return type; }
            set { type = value; }
        }
        public string ArtworkPath
        {
            get { return artworkPath; }
            set { artworkPath = value; }
        }
        public string[] ObtainMethod
        {
            get { return obtainMethod; }
            set { obtainMethod = value; }
        }
        #endregion

        #region 导出属性
        public string ArtworkPath_16x9 // 16比9的图片路径
        {
            get { return ArtworkPath.Substring(0, ArtworkPath.Length - 4) + "_16x9.png"; }
        }
        public string ArtworkPath_1x1 // 1比1的图片路径
        {
            get { return ArtworkPath.Substring(0, ArtworkPath.Length - 4) + "_1x1.png"; }
        }
        #endregion

        /// <summary>
        /// 战术人形的构造函数
        /// </summary>
        /// <param name="id">人形ID</param>
        /// <param name="name">人形名称</param>
        /// <param name="rarity">人形稀有度</param>
        /// <param name="type">人形种类</param>
        /// <param name="artwork">人形立绘</param>
        public TDollClass(int id, string name, int rarity, TDollType type, string artworkPath, string[] obtainMethod)
        {
            ID = id;
            Name = name;
            Rarity = rarity;
            Type = type;
            ArtworkPath = artworkPath;
            ObtainMethod = obtainMethod;
        }
    }
}
