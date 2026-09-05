using GestionCommerciale.Api.Contracts;
using GestionCommerciale.Api.Security;
using GestionCommerciale.Api.Validation;
using GestionCommerciale.Domain;
using System.ComponentModel;

namespace GestionCommerciale.Api.Endpoints
{
    /// <summary>
    /// Regroupe les endpoints HTTP liés à la ressource Produit, organisés via MapGroup plutôt que
    /// directement dans Program.cs. Program.cs se contente d'appeler MapClientsEndpoints().
    /// </summary>
    public static class ProductsEndPoints
    {
        /// <summary>
        /// Déclare les endpoints /products. Méthode d'extension sur WebApplication : elle s'utilise
        /// comme app.MapProductsEndpoints() depuis Program.cs.
        /// </summary>
        /// <param name="app">L'application sur laquelle les endpoints sont enregistrés.</param>
        public static void MapProductsEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/products").WithTags("Products");

            // GET /products — retourne tous les produits. IClientRepository est injecté automatiquement
            // par le conteneur DI à chaque appel.
            group.MapGet("/", async (IProductRepository repo, ICurrentOrganizationProvider organization) =>
                Results.Ok(await repo.ListAsync(organization.GetOrganizationId())))
                .WithSummary("Liste tous les produits.")
                .WithDescription("Retourne la liste des produits de l'organisation courante, sans filtre ni pagination.");


            // GET /products/{id} — retourne un produit précis.
            // si le segment d'URL n'a pas le format d'un GUID, ASP.NET Core renvoie 404 avant même
            // d'atteindre ce code.
            group.MapGet("/{id:guid}", async ([Description("Identifiant du produit à récupérer.")] Guid id, IProductRepository repo, ICurrentOrganizationProvider organization) =>
                await repo.GetAsync(id,organization.GetOrganizationId()) is { } product 
                ? Results.Ok(product) 
                : Results.NotFound())
                .WithSummary("Récupère un produit par son Id.")
                .WithDescription("Retourne 404 si aucun produit de l'organisation ne correspond à l'Id fourni.");


            // POST /products — crée un produit. "request" est le DTO reçu (texte brut désérialisé
            // depuis le JSON de la requête) : il est transformé ici en un vrai Produit du Domain,
            // avec un nouvel Id.
            group.MapPost("/", async (CreateProductRequest request, IProductRepository repo, ICurrentOrganizationProvider organization) =>
            {
                var product = new Product(Guid.NewGuid(), organization.GetOrganizationId(), request.Name, request.Price);
                await repo.AddAsync(product);
                // Results.Created renvoie un 201 avec, dans l'en-tête Location, l'URL où retrouver
                // cette ressource et qui correspond exactement au pattern du GET /products/{id:guid} ci-dessus.
                return Results.Created($"/products/{product.Id}", product);

            })
            // Fait passer "request" par CreateProductRequestValidator avant d'exécuter le code
            // ci-dessus : si les règles ne sont pas respectées, un 400 est renvoyé directement,
            // sans jamais construire de Produit ni toucher au repository.
            .AddEndpointFilter<ValidationFilter<CreateProductRequest>>()
            .WithSummary("Crée un produit.")
            .WithDescription("Valide la requête (nom, prix) avant de créer le produit, rattaché à l'organisation courante. Renvoie 400 si les règles ne sont pas respectées.");


            // PUT /products - modifie un produit. "request" est le DTO reçu 
            group.MapPut("/{id:guid}", async ([Description("Identifiant du produit à modifier.")] Guid id, CreateProductRequest request, IProductRepository repo, ICurrentOrganizationProvider organization) =>
            {
                var organizationId = organization.GetOrganizationId();
                //Est ce que le produit existe ?
                if (await repo.GetAsync(id,organizationId) is null)
                    return Results.NotFound();

                var product = new Product(id, organizationId, request.Name, request.Price);
                await repo.EditAsync(product);
                return Results.Ok(product);
            })
            // Comme pour le post, on vérifie si la modification respecte les règles
            // de validation. Sinon erreur 400.
            .AddEndpointFilter<ValidationFilter<CreateProductRequest>>()
            .WithSummary("Modifie un produit existant.")
            .WithDescription("Remplace le nom et le prix du produit correspondant à l'Id + organisation. Renvoie 404 si le produit n'existe pas.");


            // DELETE /products - supprime un produit. "request" est le DTO reçu
            group.MapDelete("/", async ([Description("Identifiant du produit à supprimer.")] Guid id, IProductRepository repo, ICurrentOrganizationProvider organization) =>
            {
                if (await repo.GetAsync(id, organization.GetOrganizationId()) is null)
                    return Results.NotFound();
                await repo.DeleteAsync(id);
                return Results.NoContent();
            })
            .WithSummary("Supprime un produit.")
            .WithDescription("Renvoie 204 si la suppression a eu lieu, 404 si le produit n'existait déjà pas (ou appartient à une autre organisation).");


        }
    }
}
