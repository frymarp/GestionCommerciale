using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    public record Order(Guid Id, Guid ClientId, DateTime CreatingTime)
    {
        /// <summary>
        /// Order status (Draft, Approved, Invoiced, Paid, Canceled)
        /// </summary>
        public StatusOrder Status { get; set; } = StatusOrder.Draft;
        /// <summary>
        /// Order lines (list)
        /// </summary>
        public List<OrderLine> Lines { get; init; } = new();


        /// <summary>
        /// Calculate the order total: (unit price * quantity) for each order line
        /// </summary>
        public decimal Total => Lines.Sum(l => l.UnitPrice * l.Quantity);
    }
}
