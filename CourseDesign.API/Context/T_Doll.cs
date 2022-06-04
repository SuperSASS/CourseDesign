namespace CourseDesign.API.Context
{
    public class TDoll : BaseEntity
    {
        public enum TypeEnum { HG, SMG, RF, AR, MG, SG }; // 人形类型的枚举

        public string Name { get; set; }
        public int Rarity { get; set; }
        public TypeEnum Type { get; set; }
        public string ArtworkPath { get; set; }
    }
}
