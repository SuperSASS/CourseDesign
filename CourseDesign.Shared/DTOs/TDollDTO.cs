namespace CourseDesign.Shared.DTOs
{
    /// <summary>
    /// TDoll（战术人形）数据实体
    /// </summary>
    public class TDollDTO : BaseDTO
    {
        public enum TDollType { HG, SMG, RF, AR, MG, SG }; // 计划类型的枚举

        private string name;
        private int rarity;
        private TDollType type;
        private string artworkPath; // 路径目前统一为“/Assets/T-Dolls/T-Doll-x.png”
        private string obtainMethod; // 用斜线"/"分割每种获取方式

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        public int Rarity
        {
            get { return rarity; }
            set { rarity = value; OnPropertyChanged(); }
        }
        public TDollType Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged(); }
        }
        public string ArtworkPath
        {
            get { return artworkPath; }
            set { artworkPath = value; OnPropertyChanged(); }
        }
        public string ObtainMethod
        {
            get { return obtainMethod; }
            set { obtainMethod = value; }
        }
    }
}
