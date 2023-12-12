using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingKTX.Models
{
    [Table("tb_file")]

    public class SqlFile
    {
        [Key]
        public long ID { get; set; }
        public string key { get; set; } = "";
        public string link { get; set; } = "";
        public string name { get; set; } = "";
        public DateTime time { get; set; }
    }
}
