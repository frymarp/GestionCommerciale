using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Product
    /// </summary>
    /// <param name="Id">Product ID</param>
    /// <param name="Name">Product name</param>
    /// <param name="Price">Product price</param>
    public record Product(Guid Id, string Name, decimal Price);
}
