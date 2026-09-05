using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Organization — l'organisation qui a accès à la gestion commerciale.
    /// </summary>
    /// <param name="Id">Identifiant unique (généré côté serveur).</param>
    /// <param name="Name">Nom de l'organisation.</param>
    public record Organization(Guid Id, string Name);
}
