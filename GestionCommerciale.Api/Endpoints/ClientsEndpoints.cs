using GestionCommerciale.Api.Contracts;
using GestionCommerciale.Api.Validation;
using GestionCommerciale.Domain;
using System.ComponentModel;

namespace GestionCommerciale.Api.Endpoints
{
    /// <summary>
    /// Regroupe les endpoints HTTP liés à la ressource Client, organisés via MapGroup plutôt que
    /// directement dans Program.cs. Program.cs se contente d'appeler MapClientsEndpoints().
    /// </summary>
    public static class ClientsEndpoints
    {
        /// <summary>
        /// Déclare les endpoints /clients. Méthode d'extension sur WebApplication : elle s'utilise
        /// comme app.MapClientsEndpoints() depuis Program.cs.
        /// </summary>
        /// <param name="app">L'application sur laquelle les endpoints sont enregistrés.</param>
        public static void MapClientsEndpoints(this WebApplication app)
        {
            // MapGroup regroupe tous les endpoints suivants sous le préfixe "/clients" et leur
            // applique WithTags("Clients"), qui les fait apparaître groupés dans Swagger.
            var group = app.MapGroup("/clients").WithTags("Clients");

            // GET /clients — retourne tous les clients. IClientRepository est injecté automatiquement
            // par le conteneur DI à chaque appel.
            group.MapGet("/", async (IClientRepository repo) =>
                Results.Ok(await repo.ListAsync()))
                .WithSummary("Liste tous les clients.")
                .WithDescription("Retourne la liste complète des clients enregistrés, sans filtre ni pagination.");


            // GET /clients/{id} — retourne un client précis.
            // si le segment d'URL n'a pas le format d'un GUID, ASP.NET Core renvoie 404 avant même
            // d'atteindre ce code. "is { } client" teste que le résultat n'est pas null et, si c'est
            // le cas, capture la valeur dans la variable "client" en une seule expression.
            group.MapGet("/{id:guid}", async ([Description("Identifiant du client à récupérer.")] Guid id, IClientRepository repo) =>
                await repo.GetAsync(id) is { } client ? Results.Ok(client) : Results.NotFound())
                .WithSummary("Récupère un client par son Id.")
                .WithDescription("Retourne 404 si aucun client ne correspond à l'Id fourni.");


            // POST /clients — crée un client. "request" est le DTO reçu (texte brut désérialisé
            // depuis le JSON de la requête) : il est transformé ici en un vrai Client du Domain,
            // avec un nouvel Id et un Email validé par son propre constructeur.
            group.MapPost("/", async (CreateClientRequest request, IClientRepository repo) =>
            {
                var client = new Client(Guid.NewGuid(), request.Name, new Email(request.Email));
                await repo.AddAsync(client);

                // Results.Created renvoie un 201 avec, dans l'en-tête Location, l'URL où retrouver
                // cette ressource et qui correspond exactement au pattern du GET /clients/{id:guid} ci-dessus.
                return Results.Created($"/clients/{client.Id}", client);
            })
            // Fait passer "request" par CreateClientRequestValidator avant d'exécuter le code
            // ci-dessus : si les règles ne sont pas respectées, un 400 est renvoyé directement,
            // sans jamais construire de Client ni toucher au repository.
            .AddEndpointFilter<ValidationFilter<CreateClientRequest>>()
            .WithSummary("Crée un client.")
            .WithDescription("Valide la requête (nom, email) avant de créer le client ; renvoie 400 si les règles ne sont pas respectées.");

        }
    }
}
