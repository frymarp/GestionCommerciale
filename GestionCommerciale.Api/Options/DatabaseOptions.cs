namespace GestionCommerciale.Api.Options
{
    /// <summary>
    /// Options pattern : accès à la base de données. Tant qu'il n'y a pas encore de vraie base
    /// connectée, ConnectionString reste vide ; une fois une base en place, la valeur sera fournie
    /// via user-secrets ou des variables d'environnement, et utilisée
    /// pour configurer le DbContext EF Core (AddDbContext).
    /// </summary>
    public class DatabaseOptions
    {
        /// <summary>Chaîne de connexion vers la base de données (serveur, base, identifiants).</summary>
        public string ConnectionString { get; set; } = "";
    }
}
