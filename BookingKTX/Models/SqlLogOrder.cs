using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingKTX.Models
{
    [Table("tb_logOrder")]
    public class SqlLogOrder
    {
        [Key]
        public long ID { get; set; }
        public SqlOrder? order { get; set; }
        public SqlUser? user { get; set; }
        public SqlAction? action { get; set; }
        public DateTime time { get; set; }
        public string note { get; set; } = "";
    }
}
