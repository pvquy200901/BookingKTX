using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingKTX.Models
{
    [Table("tb_product")]
    public class SqlProduct
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public bool isdeleted { get; set; } = false;
        public SqlShop? shop { get; set; }
    }
}
