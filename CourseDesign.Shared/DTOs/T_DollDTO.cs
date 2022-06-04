namespace CourseDesign.Shared.DTOs
{
    /// <summary>
    /// TDoll（战术人形）数据实体
    /// </summary>
    public class TDollDTO : BaseDTO
    {
        public enum TypeEnum { HG, SMG, RF, AR, MG, SG }; // 计划类型的枚举

        private string name;
        private int rarity;
        private TypeEnum type;
        private string artworkPath; /// TODO : 把图片改为数据库端存储

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
        public TypeEnum Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged(); }
        }
        public string ArtworkPath
        {
            get { return artworkPath; }
            set { artworkPath = value; OnPropertyChanged(); }
        }
    }
}
