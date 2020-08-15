using SQLite;

namespace SchneiderElectricProject.Objects
{
    [Table("Water_meter")]
    class Water_meter
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        [NotNull, Unique]
        public string Serial { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

    }
}
