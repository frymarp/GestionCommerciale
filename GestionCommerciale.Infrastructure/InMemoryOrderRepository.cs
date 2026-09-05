using GestionCommerciale.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Infrastructure
{

    /// <summary>
    /// Implémentation d'IOrderRepository qui stocke les clients en mémoire (pas de BDD)
    /// </summary>
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        /// <summary>Ajoute une commande à la liste en mémoire.</summary>
        /// <param name="order">La commande à ajouter.</param>
        /// <returns>Une tâche déjà terminée (Task.CompletedTask). Aucun travail réellement asynchrone n'a lieu ici.</returns>
        public Task AddAsync(Order order)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }

        /// <summary>Recherche une commande par son Id dans la liste en mémoire.</summary>
        /// <param name="id">Identifiant de la commande recherchée.</param>
        /// <param name="organizationId">Identifiant de l'organisation courante.</param>
        /// <returns>La commande trouvée, ou null si aucune commande de la liste n'a cet Id.</returns>
        public Task<Order?> GetAsync(Guid id, Guid organizationId) =>
            Task.FromResult(_orders.FirstOrDefault(c => c.Id == id && c.OrganizationId == organizationId));

        /// <summary>Retourne une copie de la liste de toutes les commandes en mémoire.</summary>
        /// <param name="organizationId">Identifiant de l'organisation courante.</param>
        /// <returns>Une nouvelle liste contenant toutes les commandes actuellement stockées.</returns>
        public Task<List<Order>> ListAsync(Guid organizationId) =>
            Task.FromResult(_orders.Where(c => c.OrganizationId == organizationId).ToList());

        // Task.FromResult/Task.CompletedTask : le travail ici (lire/écrire une List<T> en mémoire) est 100% synchrone et instantané.
        // Le résultat est quand même dans une Task déjà terminée, parce que la signature de la méthode (imposée par l'interface) est asynchrone.

    }
}
