using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingKTX.Models
{
    [Table("tb_order")]
    public class SqlOrder
    {
        [Key]
        public long ID { get; set; }
       // public List<SqlUser>? user { get; set; }
        public SqlCustomer? customer { get; set; }
        public string code { get; set; } = "";
        public SqlShop? shop { get; set; }
        public List<SqlProduct>? products { get; set; }
        public SqlState? state { get; set; }
        public string note { get; set; } = "";
        public double total { get; set; } = 0;
        public DateTime createdTime { get; set; }
        public DateTime lastestTime { get; set; }
        public bool isFinish { get; set; } = false;
        public bool isDelete { get; set; } = false;
    }
}
