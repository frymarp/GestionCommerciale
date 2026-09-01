using GestionCommerciale.Api.Contracts;
using GestionCommerciale.Domain;
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
                Results.Ok(await repo.ListAsync()));

            // GET /orders/{id} — retourne une commande précis.
            // si le segment d'URL n'a pas le format d'un GUID, ASP.NET Core renvoie 404 avant même
            // d'atteindre ce code.
            group.MapGet("/{id:guid}", async (Guid id, IOrderRepository repo) =>
                await repo.GetAsync(id) is { } order ? Results.Ok(order) : Results.NotFound());

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
            });

            //Pas de PUT ou de DELETE: les commandes sont historisées
            //Aussi, quand on supprime un product, les données de l'article sont gardées dans la ligne de commande (prix à un moment donné, pas forcément le prix actuel).

        }
    }
}
