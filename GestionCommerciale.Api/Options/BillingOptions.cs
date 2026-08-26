namespace GestionCommerciale.Api.Options
{
    /// <summary>
    /// Options pattern : réglages métier liés à la facturation, regroupés dans une classe typée
    /// plutôt que lus un par un depuis IConfiguration. Liée à la section "Billing" d'appsettings.json
    /// via builder.Services.Configure dans Program.cs.
    /// </summary>
    public class BillingOptions
    {
        /// <summary>Taux de TVA par défaut appliqué aux factures.</summary>
        public decimal DefaultVAT { get; set; }

        /// <summary>Préfixe utilisé pour numéroter les factures (ex : "FAC-0001").</summary>
        public string InvoicePrefix { get; set; } = "FAC-";

    }
}
