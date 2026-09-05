using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Produit. Record immuable comparé par valeur : deux Product avec
    /// les mêmes Id/Name/Price sont considérés égaux.
    /// </summary>
    /// <param name="Id">Identifiant unique du produit.</param>
    /// <param name="OrganizationId">Identifiant de l'organisation.</param>
    /// <param name="Name">Nom du produit.</param>
    /// <param name="Price">Prix courant du produit — peut évoluer dans le temps (et n'affectera pas le prix des commandes déjà crées).</param>
    public record Product(Guid Id,Guid OrganizationId, string Name, decimal Price);
}
