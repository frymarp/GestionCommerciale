using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Client — une entité client du Domain.
    /// C'est un record : deux Client avec les mêmes valeurs (Id, Name, Email) sont considérés égaux,
    /// et une fois construit, ses propriétés positionnelles (Id, Name, Email) ne peuvent plus changer.
    /// </summary>
    /// <param name="Id">Identifiant unique (généré côté serveur).</param>
    /// <param name="OrganizationId">Identifiant de l'organisation.</param>
    /// <param name="Name">Nom du client.</param>
    /// <param name="Email">Value object Email(voir Email.cs).</param>
    public record Client(Guid Id, Guid OrganizationId, string Name, Email Email);
}
