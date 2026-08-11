# GestionCommerciale

Application de gestion commerciale (Clients → Produits → Commandes → Factures → Historique).
Le but était de découvrir les dernières features .NET 10, tout en travaillant sur une application dont je connais le fonctionnel.

## Stack technique

- .NET 10 / C# 14
- ASP.NET Core (Web API)
- Entity Framework Core
- Blazor (interface de démonstration)
- xUnit (tests)

## Statut

Projet en cours de construction, mise en place de l'environnement et de la structure de solution.

## Structure de la solution

- `GestionCommerciale.Domain` — règles métier, sans dépendance externe
- `GestionCommerciale.Infrastructure` — accès aux données, EF Core
- `GestionCommerciale.Api` — API REST en ASP.NET Core
- `GestionCommerciale.Tests` — tests unitaires et d'intégration

## Lancer le projet

```bash
dotnet build
dotnet run --project GestionCommerciale.Api
```
