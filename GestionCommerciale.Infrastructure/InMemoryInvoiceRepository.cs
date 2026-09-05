using GestionCommerciale.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GestionCommerciale.Infrastructure
{
    /// <summary>
    /// Implémentation d'IInvoiceRepository qui stocke les factures en mémoire (pas de BDD)
    /// </summary>
    public class InMemoryInvoiceRepository : IInvoiceRepository
    {
        private readonly List<Invoice> _invoices = new();

        /// <summary>Retourne toutes les factures.</summary>
        /// <param name="organizationId">Identifiant de l'organisation courante.</param>
        /// <returns>La liste de toutes les factures existantes (vide si aucun).</returns>
        public Task<List<Invoice>> ListAsync(Guid organizationId) => Task.FromResult(_invoices.Where(f => f.OrganizationId == organizationId).ToList());

        /// <summary>Recherche un client par son Id dans la liste en mémoire.</summary>
        /// <param name="id">Identifiant du client recherché.</param>
        /// <param name="organizationId">Identifiant de l'organisation courante.</param>
        /// <returns>Le client trouvé, ou null si aucun client de la liste n'a cet Id.</returns>
        public Task<Invoice?> GetAsync(Guid id, Guid organizationId) =>
            Task.FromResult(_invoices.FirstOrDefault(f => f.Id == id && f.OrganizationId == organizationId));

        /// <summary>Ajoute une facture à la liste en mémoire.</summary>
        /// <param name="invoice">La facture à ajouter.</param>
        /// <returns>Une tâche déjà terminée (Task.CompletedTask). Aucun travail asynchrone n'a lieu ici.</returns>
        public Task AddAsync(Invoice invoice)
        {
            _invoices.Add(invoice);
            return Task.CompletedTask;
        }
    }

}
