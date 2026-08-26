using GestionCommerciale.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Infrastructure
{
    /// <summary>
    /// Implémentation d'IClientRepository qui stocke les clients en mémoire (pas de BDD)
    /// </summary>
    public class InMemoryClientRepository : IClientRepository
    {
        private readonly List<Client> _clients = new();

        /// <summary>Ajoute un client à la liste en mémoire.</summary>
        /// <param name="client">Le client à ajouter.</param>
        /// <returns>Une tâche déjà terminée (Task.CompletedTask). Aucun travail asynchrone n'a lieu ici.</returns>
        public Task AddAsync(Client client)
        {
            _clients.Add(client);
            return Task.CompletedTask;
        }

        /// <summary>Recherche un client par son Id dans la liste en mémoire.</summary>
        /// <param name="id">Identifiant du client recherché.</param>
        /// <returns>Le client trouvé, ou null si aucun client de la liste n'a cet Id.</returns>
        public Task<Client?> GetAsync(Guid id) =>
            Task.FromResult(_clients.FirstOrDefault(c => c.Id == id));

        /// <summary>Retourne une copie de la liste de tous les clients en mémoire.</summary>
        /// <returns>Une nouvelle liste contenant tous les clients actuellement stockés.</returns>
        public Task<List<Client>> ListAsync() =>
            Task.FromResult(_clients.ToList());

        // Task.FromResult/Task.CompletedTask : le travail ici (lire/écrire une List<T> en mémoire) est 100% synchrone et instantané.
        // Le résultat est quand même dans une Task déjà terminée, parce que la signature de la méthode (imposée par l'interface) est asynchrone.
    }
}
