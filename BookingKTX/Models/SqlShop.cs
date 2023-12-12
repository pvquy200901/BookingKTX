using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingKTX.Models
{
    [Table("tb_shop")]
    public class SqlShop
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public bool isdeleted { get; set; } = false;
        public DateTime createdTime { get; set; }
        public DateTime lastestTime { get; set; }
        public List<SqlProduct>? products { get; set; }
        public List<SqlUser>? users { get; set; }
        public SqlType? type { get; set; }
    }
}
