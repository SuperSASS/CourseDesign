namespace CourseDesign.API.Context
{
    // 战术人形表
    public class TDoll : BaseEntity
    {
        public enum TypeEnum { HG, SMG, RF, AR, MG, SG }; // 人形类型的枚举

        /// <summary>
        /// 战术人形名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 稀有度
        /// </summary>
        public int Rarity { get; set; }
        /// <summary>
        /// 种类
        /// </summary>
        public TypeEnum Type { get; set; }
        /// <summary>
        /// 立绘在本地的路径
        /// </summary>
        public string ArtworkPath { get; set; }
    }
}
