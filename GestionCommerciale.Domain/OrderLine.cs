using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Ligne de commande : un produit commandé, avec sa quantité et surtout son prix unitaire figé
    /// au moment de la commande. Le prix est volontairement recopié ici plutôt que d'aller relire
    /// Product.Price à chaque fois : si le prix du produit change plus tard, les commandes déjà
    /// passées ne doivent pas être affectées rétroactivement.
    /// </summary>
    /// <param name="ProductId">Identifiant du produit commandé.</param>
    /// <param name="UnitPrice">Prix unitaire figé au moment de la commande.</param>
    /// <param name="Quantity">Quantité commandée.</param>
    public record OrderLine(Guid ProductId, decimal UnitPrice, int Quantity);
}
