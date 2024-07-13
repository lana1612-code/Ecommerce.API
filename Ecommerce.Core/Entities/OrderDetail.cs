using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Orders))]
        public int OrderId { get; set; }
        [ForeignKey(nameof(Products))]
        public int ProductId { get; set; }
        public float Price { get; set; }
        public int  Quantity { get; set; }

        public virtual Orders? Orders { get; set; }
        public virtual Products? Products { get; set; }


    }
}
