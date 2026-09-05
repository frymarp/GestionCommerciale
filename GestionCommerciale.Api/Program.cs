using FluentValidation;
using GestionCommerciale.Api.Endpoints;
using GestionCommerciale.Api.Options;
using GestionCommerciale.Api.Security;
using GestionCommerciale.Api.Validation;
using GestionCommerciale.Domain;
using GestionCommerciale.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Repositories : version en mémoire. Enregistrés en Singleton (une seule instance pour toute la vie
// de l'application) plutôt qu'en Scoped (une instance par requête) : la liste interne de chaque
// repository doit survivre d'une requête à l'autre pour que les données ajoutées restent visibles
// par la suite => avec un Scoped, une nouvelle liste vide serait créée à chaque appel.
builder.Services.AddSingleton<IClientRepository, InMemoryClientRepository>();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton<IInvoiceRepository, InMemoryInvoiceRepository>();

// Options pattern : lie chaque classe à sa section d'appsettings.json, ce qui la rend injectable
// ailleurs via IOptions<T> (IOptions<DatabaseOptions>).
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("Database"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<BillingOptions>(builder.Configuration.GetSection("Billing"));

// Scanne l'assembly courante et enregistre tous les validateurs FluentValidation trouvés
//CreateClientRequestValidator sert juste de "repère" pour indiquer quel assembly scanner.
builder.Services.AddValidatorsFromAssemblyContaining<CreateClientRequestValidator>();

// Enregistre le conteneur DI qui va générer le document OpenAPI (json qui décrit les routes, params etc...)
builder.Services.AddOpenApi(options =>
{
    //Ajout des tags 
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Tags = new HashSet<OpenApiTag>
    {
        new() { Name = "Clients", Description = "Gestion des clients : création et consultation." },
        new() { Name = "Products", Description = "Catalogue des produits : création, modification, suppression." },
        new() { Name = "Orders", Description = "Création des commandes à partir d'un client et de produits existants." },
        new() { Name = "Invoices", Description = "Consultation des factures générées, et action de paiement." },
    };
        return Task.CompletedTask;
    });
});

//TEMPORAIRE: Accès à l'entete HTTP pour récupérer l'identifiant de l'organisation
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentOrganizationProvider, HeaderOrganizationProvider>();




// builder.Build() fige la configuration ci-dessus et produit "app", l'objet représentant
// l'application prête à recevoir des requêtes. app.Run() démarre le serveur et bloque le programme
// à cet endroit tant que l'application tourne.
var app = builder.Build();

//Groupes d'endpoints
app.MapClientsEndpoints();
app.MapProductsEndpoints();
app.MapOrdersEndpoints();
app.MapInvoicesEndpoints();

//endpoint Http qui permet de récupéré le document généré open api (format json brut)
app.MapOpenApi();

//endpoint Html de l'interface graphique Scalar (pareil qu'au dessus mais plus simple pour les users)
app.MapScalarApiReference();

//Middleware de gestion d'erreur (recupère celles dans Orders POST quand un client est introuvable par ex)
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Title = "Une erreur inattendue est survenue",
            Status = StatusCodes.Status500InternalServerError
        });
    });
});


app.Run();

