using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Order line
    /// </summary>
    /// <param name="ProductId">Product ID</param>
    /// <param name="UnitPrice">Product unit price (in €)</param>
    /// <param name="Quantity">Product quantity</param>
    public record OrderLine(Guid ProductId, decimal UnitPrice, int Quantity);
}
