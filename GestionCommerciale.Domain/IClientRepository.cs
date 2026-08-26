using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Contrat du repository Client : le Domain décrit ce qui doit être possible (lister, obtenir,
    /// ajouter un client), sans jamais dire comment ce sera fait. Aucune dépendance à EF Core, à une
    /// base de données ou à quoi que ce soit de technique.
    /// Toutes les méthodes sont asynchrones car une implémentation persistante
    /// fera de vrais appels réseau vers la base de données.
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>Retourne tous les clients.</summary>
        /// <returns>La liste de tous les clients existants (vide si aucun).</returns>
        Task<List<Client>> ListAsync();

        /// <summary>Retourne un client par son Id, ou null s'il n'existe pas.</summary>
        /// <param name="id">Identifiant du client recherché.</param>
        /// <returns>Le client correspondant, ou null si aucun client ne porte cet Id.</returns>
        Task<Client?> GetAsync(Guid id);

        /// <summary>Ajoute un nouveau client.</summary>
        /// <param name="client">Le client à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone (pas de valeur de retour).</returns>
        Task AddAsync(Client client);

    }
}
