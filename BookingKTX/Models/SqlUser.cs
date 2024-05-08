using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingKTX.Models
{
    [Table("tb_user")]
    public class SqlUser
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        public string token { get; set; } = "";
        public string displayName { get; set; } = "";
        public bool isdeleted { get; set; } = false;
        public List<string>? images { get; set; }
        public string idHub { get; set; } = "";
        public string phoneNumber { get; set; } = "";
        public string avatar { get; set; } = "";
        public SqlRole? role { get; set; }
        public List<SqlOrder>? orderShippers { get; set; }
        public SqlShop? shop { get; set; }
    }
}
