namespace GestionCommerciale.Api.Options
{
    /// <summary>
    /// Options pattern : réglages de l'authentification JWT, destinés à construire les
    /// TokenValidationParameters (Issuer, Audience, clé de signature) au lieu de valeurs éparpillées
    /// en dur dans le code.
    /// </summary>
    public class JwtOptions
    {
        /// <summary>Émetteur du jeton qui délivre les jetons JWT.</summary>
        public string Issuer { get; set; } = "";

        /// <summary>Audience — qui a le droit d'utiliser ces jetons.</summary>
        public string Audience { get; set; } = "";

        /// <summary>
        /// Clé secrète servant à signer/valider les jetons. Une vraie valeur (jamais celle par
        /// défaut vide) doit venir de user-secrets en dev, d'une variable d'environnement en prod —
        /// jamais commitée en clair dans appsettings.json.
        /// </summary>
        public string SecretKey { get; set; } = "";

        /// <summary>Durée de validité d'un jeton, en minutes, avant qu'il faille s'authentifier à nouveau.</summary>
        public int ValidityDurationInMinutes { get; set; } = 60;
    }
}
