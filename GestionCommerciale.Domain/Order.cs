using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Commande. L'identité (Id), le client associé (ClientId) et la date de création (date)
    /// sont figés à la construction via la syntaxe positionnelle du record — ils ne changeront jamais.
    /// Le Statut et les Lignes, en revanche, évoluent légitimement dans le temps (init/set explicites
    /// ci-dessous), ce qui justifie qu'ils ne soient pas dans la liste positionnelle du record.
    /// </summary>
    /// <param name="Id">Identifiant unique (généré côté serveur).</param>
    /// <param name="OrganizationId">Identifiant de l'organisation.</param>
    /// <param name="ClientId">Id du client</param>
    /// <param name="date">Date de création</param>
    public record Order(Guid Id, Guid OrganizationId, Guid ClientId, DateTime date)
    {
        /// <summary>
        /// Statut de la commande (Draft, Approved, Invoiced, Paid, Canceled).
        /// Mutable
        /// Les transitions autorisées sont contrôlées séparément dans OrderTransitions (StatusOrder.cs).
        /// </summary>
        public OrderStatus Status { get; set; } = OrderStatus.Draft;

        /// <summary>
        /// Lignes de la commande.
        /// Mutable
        /// </summary>
        public List<OrderLine> Lines { get; init; } = new();

        /// <summary>
        /// Renvoie le prix total de la commande : jamais stockée, toujours recalculée
        /// Somme, pour chaque ligne, du produit du prix unitaire fois la quantité
        /// </summary>
        public decimal Total => Lines.Sum(l => l.UnitPrice * l.Quantity);
    }
}
