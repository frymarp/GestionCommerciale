using FluentValidation;
using GestionCommerciale.Api.Options;
using GestionCommerciale.Api.Validation;
using GestionCommerciale.Domain;
using GestionCommerciale.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Repositories : version en mémoire. Enregistrés en Singleton (une seule instance pour toute la vie
// de l'application) plutôt qu'en Scoped (une instance par requête) : la liste interne de chaque
// repository doit survivre d'une requête à l'autre pour que les données ajoutées restent visibles
// par la suite => avec un Scoped, une nouvelle liste vide serait créée à chaque appel.
builder.Services.AddSingleton<IClientRepository, InMemoryClientRepository>();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

// Options pattern : lie chaque classe à sa section d'appsettings.json, ce qui la rend injectable
// ailleurs via IOptions<T> (IOptions<DatabaseOptions>).
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("Database"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<BillingOptions>(builder.Configuration.GetSection("Billing"));

// Scanne l'assembly courante et enregistre tous les validateurs FluentValidation trouvés
//CreateClientRequestValidator sert juste de "repère" pour indiquer quel assembly scanner.
builder.Services.AddValidatorsFromAssemblyContaining<CreateClientRequestValidator>();

// builder.Build() fige la configuration ci-dessus et produit "app", l'objet représentant
// l'application prête à recevoir des requêtes. app.Run() démarre le serveur et bloque le programme
// à cet endroit tant que l'application tourne.
var app = builder.Build();

app.Run();
