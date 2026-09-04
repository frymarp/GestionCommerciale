using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Facture. L'identité (Id), la commande associée (ClientId) et la date de création (Date)
    /// sont figés à la construction via la syntaxe positionnelle du record — ils ne changeront jamais.
    /// Le Statut et les Lignes, en revanche, évoluent légitimement dans le temps (init/set explicites
    /// ci-dessous), ce qui justifie qu'ils ne soient pas dans la liste positionnelle du record.
    /// </summary>
    /// <param name="Id">Identifiant unique (généré côté serveur).</param>
    /// <param name="OrderId">Id de la commande</param>
    /// <param name="date">Date de création</param>
    public record Invoice(Guid Id, Guid OrderId, DateTime createdDate)
    {
        /// <summary>
        /// Statut de la facture (Pending,Paid, Cancelled).
        /// Mutable
        /// Les transitions autorisées sont contrôlées séparément dans OrderTransitions (StatusInvoice.cs).
        /// </summary>
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;
        /// <summary>
        /// Lignes de la facture.
        /// Mutable
        /// </summary>
        public List<InvoiceLine> Lines { get; init; } = new();
        /// <summary>
        /// Renvoie le prix total de la facture : jamais stockée, toujours recalculée
        /// Somme, pour chaque ligne, du produit du prix unitaire fois la quantité
        /// </summary>
        public decimal Total => Lines.Sum(l => l.UnitPrice * l.Quantity);
    }

}
