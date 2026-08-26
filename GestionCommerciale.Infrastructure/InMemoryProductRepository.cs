using GestionCommerciale.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Infrastructure
{
    /// <summary>
    /// Implémentation d'IProductRepository qui stocke les clients en mémoire (pas de BDD)
    /// </summary>
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();

        /// <summary>Ajoute un produit à la liste en mémoire.</summary>
        /// <param name="product">Le produit à ajouter.</param>
        /// <returns>Une tâche déjà terminée (Task.CompletedTask). Aucun travail réellement asynchrone n'a lieu ici.</returns>
        public Task AddAsync(Product product)
        {
            _products.Add(product);
            return Task.CompletedTask;
        }

        /// <summary>Recherche un produit par son Id dans la liste en mémoire.</summary>
        /// <param name="id">Identifiant du produit recherché.</param>
        /// <returns>Le produit trouvé, ou null si aucun produit de la liste ne porte cet Id.</returns>
        public Task<Product?> GetAsync(Guid id) =>
            Task.FromResult(_products.FirstOrDefault(c => c.Id == id));

        /// <summary>Retourne une copie de la liste de tous les produits en mémoire.</summary>
        /// <returns>Une nouvelle liste contenant tous les produits actuellement stockés.</returns>
        public Task<List<Product>> ListAsync() =>
            Task.FromResult(_products.ToList());

        // Task.FromResult/Task.CompletedTask : le travail ici (lire/écrire une List<T> en mémoire) est 100% synchrone et instantané.
        // Le résultat est quand même dans une Task déjà terminée, parce que la signature de la méthode (imposée par l'interface) est asynchrone.
    }
}
