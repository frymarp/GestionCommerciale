using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Contrat du repository Invoice : le Domain décrit ce qui doit être possible (lister, obtenir,
    /// ajouter une facture), sans jamais dire comment ce sera fait. Aucune dépendance à EF Core, à une
    /// base de données ou à quoi que ce soit de technique.
    /// Toutes les méthodes sont asynchrones car une implémentation persistante
    /// fera de vrais appels réseau vers la base de données.
    /// </summary>
    public interface IInvoiceRepository
    {
        /// <summary>Retourne toutes les factures.</summary>
        /// <returns>La liste de toutes les factures existantes (vide si aucun).</returns>
        Task<List<Invoice>> ListAsync();

        /// <summary>Retourne une facture par son Id, ou null s'il n'existe pas.</summary>
        /// <param name="id">Identifiant de la facture recherchée.</param>
        /// <returns>La facture correspondante, ou null si aucune facture ne porte cet Id.</returns>
        Task<Invoice?> GetAsync(Guid id);

        /// <summary>Ajoute une nouvelle facture.</summary>
        /// <param name="invoice">La facture à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone (pas de valeur de retour).</returns>
        Task AddAsync(Invoice invoice);

    }
}
