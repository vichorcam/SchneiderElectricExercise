using SQLite;

namespace SchneiderElectricProject.Objects
{
    [Table("Electricity_meter")]
    class Electricity_meter
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        [NotNull, Unique]
        public string Serial { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

    }
}
