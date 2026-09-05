namespace GestionCommerciale.Api.Security
{
    /// <summary>
    /// IMPLEMENTATION TEMPORAIRE (attente authentification JWT)
    /// NON SECURISÉ !!!!!! (juste pour tester le cloisonnement entre organisations)
    /// Permet de lire l'id de l'organisation depuis un entête HTTP dédié.
    /// </summary>
    public class HeaderOrganizationProvider : ICurrentOrganizationProvider
    {
        // Permet de donner accès à la requête HTTP courante 
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="httpContextAccessor">Accès requête HTTP</param>
        public HeaderOrganizationProvider(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        /// <summary>
        /// Récupère l'identifiant d'organization dans l'entête HTTP
        /// </summary>
        /// <returns>L'identifiant de l'organisation</returns>
        /// <exception cref="InvalidOperationException">Organisation manquante dans l'entête ou invalide.</exception>
        public Guid GetOrganizationId()
        {
            var valeur = _httpContextAccessor.HttpContext?.Request.Headers["X-Organisation-Id"].FirstOrDefault();
            return Guid.TryParse(valeur, out var id)
                ? id
                : throw new InvalidOperationException("En-tête X-Organisation-Id manquant ou invalide.");
        }

    }
}
