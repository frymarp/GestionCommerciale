using GestionCommerciale.Api.Contracts;
using GestionCommerciale.Domain;
using System.ComponentModel;
using System.Text.RegularExpressions;
namespace GestionCommerciale.Api.Endpoints
{
    /// <summary>
    /// Regroupe les endpoints HTTP liés à la ressource Order, organisés via MapGroup plutôt que
    /// directement dans Program.cs. Program.cs se contente d'appeler MapOrdersEndpoints().
    /// </summary>
    /// 
    public static class OrdersEndpoints 
    {

        /// <summary>
        /// Déclare les endpoints /orders. Méthode d'extension sur WebApplication : elle s'utilise
        /// comme app.MapOrdersEndpoints() depuis Program.cs.
        /// </summary>
        /// <param name="app">L'application sur laquelle les endpoints sont enregistrés.</param>
        public static void MapOrdersEndpoints(this WebApplication app)
        {
            // MapGroup regroupe tous les endpoints suivants sous le préfixe "/orders" et leur
            // applique WithTags("Orders"), qui les fait apparaître groupés dans Swagger.
            var group = app.MapGroup("/orders").WithTags("Orders");

            // GET /orders — retourne tous les commandes. IOrderRepository est injecté automatiquement
            // par le conteneur DI à chaque appel.
            group.MapGet("/", async (IOrderRepository repo) =>
                Results.Ok(await repo.ListAsync()))
                .WithSummary("Liste toute les commandes.")
                .WithDescription("Retourne la liste complète des commandes enregistrées, sans filtre ni pagination.");


            // GET /orders/{id} — retourne une commande précis.
            // si le segment d'URL n'a pas le format d'un GUID, ASP.NET Core renvoie 404 avant même
            // d'atteindre ce code.
            group.MapGet("/{id:guid}", async ([Description("Identifiant de la commande à récupérer.")] Guid id, IOrderRepository repo) =>
                await repo.GetAsync(id) is { } order ? 
                Results.Ok(order)
                : Results.NotFound())
                .WithSummary("Récupère une commande par son Id.")
                .WithDescription("Retourne 404 si aucune commande ne correspond à l'Id fourni.");



            // POST /orders — crée une commande. "request" est le DTO reçu (texte brut désérialisé
            // depuis le JSON de la requête) : il est transformé ici en une vraie commande du Domain,
            // avec un nouvel Id et un Email validé par son propre constructeur.
            //Crée aussi les lignes de commande (via le createOrderRequest)
            group.MapPost("/", async (CreateOrderRequest request, IOrderRepository orders, IClientRepository clients, IProductRepository products) =>
            {
                // Recherche du client
                // Si erreur, l'exception n'est levée nulle part => le middleware s'en occupera
                var client = await clients.GetAsync(request.ClientId) ?? 
                    throw new InvalidOperationException("Client introuvable");
                var lines = new List<OrderLine>();
                foreach (var line in request.Lines)
                {
                    // Création des lignes de commande
                    // Si erreur, l'exception n'est levée nulle part => le middleware s'en occupera
                    var product = await products.GetAsync(line.ProductId) ??
                        throw new InvalidOperationException("Produit introuvable");
                    lines.Add(new OrderLine(product.Id, product.Price, line.Quantity));
                }
                //Création de la commande
                var order = new Order(Guid.NewGuid(), client.Id, DateTime.UtcNow)
                {
                    Lines = lines //Lines pas dans le constructeur du record (car peuvent évoluer, comme le statut de la commande)
                    //D'ailleurs elles sont au status "Draft" automatiquement à la construction
                };
                await orders.AddAsync(order);

                return Results.Created($"orders/{order.Id}", order);
            })
            .WithSummary("Crée une commande.")
            .WithDescription("Vérifie que le client et chaque produit référencé existent, reprend le prix courant de chaque produit dans les lignes, puis calcule le Total à partir de ces lignes.");

            // POST /orders{id}/approve — passe une commande à validée
            group.MapPost("/{id:guid}/approve", async ([Description("Identifiant de la commande à valider.")] Guid id, IOrderRepository orders) =>
            {
                //Recherche de la commande
                if (await orders.GetAsync(id) is not { } order)
                    return Results.NotFound();

                //On passe à validée que si on peut
                if (!OrderTransitions.CanTransitionTo(order.Status, OrderStatus.Approved))
                    return Results.Conflict($"Impossible de passer de la commande à l'état validé.");

                //On passe la commande à approuvée
                order.Status = OrderStatus.Approved;
                return Results.Ok(order);
            })
            .WithSummary("Valide une commande.")
            .WithDescription("Fait passer la commande de Brouillon à Validée ; renvoie 409 si la transition n'est pas autorisée depuis le statut actuel.");

            // POST /orders{id}/invoice — passe une commande à facturée et crée une facture associée
            group.MapPost("/{id:guid}/invoice", async ([Description("Identifiant de la commande à facturer.")] Guid id, IOrderRepository orders, IInvoiceRepository invoices) =>
            {
                //Recherche de la commande
                if (await orders.GetAsync(id) is not { } order)
                    return Results.NotFound();

                //Seulement si on peut approuver la commande
                if (!OrderTransitions.CanTransitionTo(order.Status, OrderStatus.Invoiced))
                    return Results.Conflict($"Impossible de passer la commande à Facturée.");

                //Crée les lignes de facture à partir des lignes de commande
                var invoiceLines = order.Lines
                    .Select(l => new InvoiceLine(l.ProductId, l.UnitPrice, l.Quantity))
                    .ToList();

                //Crée la facture
                var invoice = new Invoice(Guid.NewGuid(), order.Id, DateTime.UtcNow) { Lines = invoiceLines };
                await invoices.AddAsync(invoice);

                //Passe la commande à facturée
                order.Status = OrderStatus.Invoiced;
                return Results.Created($"/orders/{order.Id}", order);
            })
            .WithSummary("Facture une commande.")
            .WithDescription("Copie les lignes de la commande dans une nouvelle facture, puis fait passer la commande au statut Facturee ; renvoie 409 si la transition n'est pas autorisée.");


            //Pas de PUT ou de DELETE: les commandes sont historisées
            //Aussi, quand on supprime un product, les données de l'article sont gardées dans la ligne de commande (prix à un moment donné, pas forcément le prix actuel).

        }
    }
}
