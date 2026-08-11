using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Client
    /// </summary>
    /// <param name="Id">Client id</param>
    /// <param name="Name">Name</param>
    /// <param name="Email">Email</param>
    public record Client(Guid Id, string Name, Email Email);
}
