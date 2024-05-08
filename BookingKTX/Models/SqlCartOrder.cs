﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingKTX.Models
{
    [Table("tb_cartOrder")]

    public class SqlCartOrder
    {
        [Key]
        public long ID { get; set; }
        public int quantity { get; set; }
        public SqlProduct? product { get; set; }
        public bool isdeleted { get; set; } = false;
    }
}
