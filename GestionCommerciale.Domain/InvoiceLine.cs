using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Ligne de facture : un produit commandé, avec sa quantité et surtout son prix unitaire figé
    /// au moment de la facture. Le prix est volontairement recopié ici plutôt que d'aller relire
    /// Product.Qi le prix du produit change plus tard, les commandes déjà
    /// passées ne doivent pas être affectées rétroactivement.
    /// </summary>
    /// <param name="ProductId">Identifiant du produit facturée.</param>
    /// <param name="UnitPrice">Prix unitaire figé au moment de la facture.</param>
    /// <param name="Quantity">Quantité facturée.</param>
    public record InvoiceLine(Guid ProductId, decimal UnitPrice, int Quantity);
}
