using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Contrat du repository Product : le Domain décrit ce qui doit être possible (lister, obtenir,
    /// ajouter un produit), sans jamais dire comment ce sera fait. Aucune dépendance à EF Core, à une
    /// base de données ou à quoi que ce soit de technique.
    /// Toutes les méthodes sont asynchrones car une implémentation persistante
    /// fera de vrais appels réseau vers la base de données.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>Retourne tous les produits.</summary>
        /// <param name="organizationId">Identifiant de l'organisation courante.</param>
        /// <returns>La liste de tous les produits existants (vide si aucun).</returns>
        Task<List<Product>> ListAsync(Guid organizationId);

        /// <summary>Retourne un produit par son Id, ou null s'il n'existe pas.</summary>
        /// <param name="id">Identifiant du produit recherché.</param>
        /// <param name="organizationId">Identifiant de l'organisation courante.</param>
        /// <returns>Le produit correspondant, ou null si aucun produit ne porte cet Id.</returns>
        Task<Product?> GetAsync(Guid id, Guid organizationId);

        /// <summary>Ajoute un nouveau produit.</summary>
        /// <param name="product">Le produit à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone (pas de valeur de retour).</returns>
        Task AddAsync(Product product);

        /// <summary>Modifie un nouveau produit.</summary>
        /// <param name="product">Le produit à modifier.</param>
        /// <returns>Une tâche représentant l'opération asynchrone (pas de valeur de retour).</returns>
        Task EditAsync(Product product);

        /// <summary>Supprime un nouveau produit.</summary>
        /// <param name="id">L'ID du produit à supprimer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone (pas de valeur de retour).</returns>
        Task DeleteAsync(Guid id);

    }
}
