using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingKTX.Models
{
    [Table("tb_customer")]
    public class SqlCustomer
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string username { get; set; } = "";
        public string token { get; set; } = "";
        public string password { get; set; } = "";
        public string phone { get; set; } = "";
        public string name { get; set; } = "";
        public string idhub { get; set; } = "";
        public string avarta { get; set; } = "";
        public string address { get; set; } = "";
        public List<SqlOrder>? orders { get; set; }
        public SqlCart? cart { get; set; }
        public bool isdeleted { get; set; } = false;
    }
}
