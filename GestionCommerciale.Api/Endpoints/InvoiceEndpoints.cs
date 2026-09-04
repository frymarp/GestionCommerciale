using GestionCommerciale.Domain;
using System.ComponentModel;
namespace GestionCommerciale.Api.Endpoints
{
    /// <summary>
    /// Regroupe les endpoints HTTP liés à la ressource Invoice, organisés via MapGroup plutôt que
    /// directement dans Program.cs. Program.cs se contente d'appeler MapInvoicesEndpoints().
    /// </summary>
    public static class InvoicesEndpoints
    {
        /// <summary>
        /// Déclare les endpoints /invoices. Méthode d'extension sur WebApplication : elle s'utilise
        /// comme app.MapClientsEndpoints() depuis Program.cs.
        /// </summary>
        /// <param name="app">L'application sur laquelle les endpoints sont enregistrés.</param>
        public static void MapInvoicesEndpoints(this WebApplication app)
        {
            // MapGroup regroupe tous les endpoints suivants sous le préfixe "/invoices" et leur
            // applique WithTags("Invoices"), qui les fait apparaître groupés dans Swagger.
            var group = app.MapGroup("/invoices").WithTags("Invoices");

            // GET /invoices — retourne toutes les factures. IInvoiceRepository est injecté automatiquement
            // par le conteneur DI à chaque appel.
            group.MapGet("/", async (IInvoiceRepository repo) =>
                Results.Ok(await repo.ListAsync()))
                .WithSummary("Liste toutes les factures.")
                .WithDescription("Retourne la liste complète des factures enregistrées, sans filtre ni pagination.");

            // GET /invoices/{id} — retourne une facture précise.
            group.MapGet("/{id:guid}", async ([Description("Identifiant de la facture à récupérer.")] Guid id, IInvoiceRepository repo) =>
                await repo.GetAsync(id) is { } facture
                    ? Results.Ok(facture)
                    : Results.NotFound())
                .WithSummary("Récupère une facture par son Id.")
                .WithDescription("Retourne 404 si aucune facture ne correspond à l'Id fourni.");

            // POST /invoices{id}/invoice — passe une commande à payée
            group.MapPost("/{id:guid}/pay", async ([Description("Identifiant de la facture à passer à payée.")] Guid id, IInvoiceRepository repo) =>
            {
                //Récupère la facture
                if (await repo.GetAsync(id) is not { } invoice)
                    return Results.NotFound();

                if (!InvoiceTransitions.CanTransitionTo(invoice.Status, InvoiceStatus.Paid))
                    return Results.Conflict($"Impossible de passer la facture à Payée.");

                invoice.Status = InvoiceStatus.Paid;
                return Results.Ok(invoice);
            })
            .WithSummary("Marque une facture comme payée.")
            .WithDescription("Fait passer la facture au statut Payée ; renvoie 409 si la transition n'est pas autorisée depuis le statut actuel.");
        }
    }

}
