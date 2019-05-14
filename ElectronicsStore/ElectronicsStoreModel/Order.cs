using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicsStoreModel
{
    public class Order
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public decimal Sum { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime DateCreate { get; set; }

        public virtual Client Client { get; set; }

        [ForeignKey("OrderId")]
        public virtual List<OrderProduct> OrderProducts { get; set; }

        [ForeignKey("OrderId")]
        public virtual List<OrderPayment> OrderPayments { get; set; }
    }
}
