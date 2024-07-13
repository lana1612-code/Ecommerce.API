using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string? UserName { get; set; }


    }
}
