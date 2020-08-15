using SQLite;

namespace SchneiderElectricProject.Objects
{
    [Table("Gateway")]
    class Gateway
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        [NotNull, Unique]
        public string Serial { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }


    }
}
