using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Contrat du repository Order : le Domain décrit ce qui doit être possible (lister, obtenir,
    /// ajouter une commande), sans jamais dire comment ce sera fait. Aucune dépendance à EF Core, à une
    /// base de données ou à quoi que ce soit de technique.
    /// Toutes les méthodes sont asynchrones car une implémentation persistante
    /// fera de vrais appels réseau vers la base de données.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>Retourne toutes les commandes.</summary>
        /// <param name="organizationId">Identifiant de l'organisation courante.</param>
        /// <returns>La liste de toutes les commandes existantes (vide si aucune).</returns>
        Task<List<Order>> ListAsync(Guid organizationId);

        /// <summary>Retourne une commande par son Id, ou null si elle n'existe pas.</summary>
        /// <param name="id">Identifiant de la commande recherchée.</param>
        /// <param name="organizationId">Identifiant de l'organisation courante.</param>
        /// <returns>La commande correspondante, ou null si aucune commande ne porte cet Id.</returns>
        Task<Order?> GetAsync(Guid id, Guid organizationId);

        /// <summary>Ajoute une nouvelle commande.</summary>
        /// <param name="order">La commande à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone (pas de valeur de retour).</returns>
        Task AddAsync(Order order);

    }
}
