using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class Orders
    {
        public int Id { get; set; }
        [ForeignKey(nameof(localUser))]
        public string LocalUserId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual LocalUser? localUser { get; set; }
         
        public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new HashSet<OrderDetail>();

    }
}
