using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingKTX.Models
{
    [Table("tb_cart")]

    public class SqlCart
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public List<SqlCartProduct>? cartProducts { get; set; }
       // public SqlCustomer? customer { get; set; }
        public bool isdeleted { get; set; } = false;
    }
}
