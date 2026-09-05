namespace GestionCommerciale.Api.Security
{
    /// <summary>Gestion de l'organisation: permet de déterminer l'organisation de l'appelant</summary>
    public interface ICurrentOrganizationProvider
    {
        /// <summary>Retourne l'organisation auquel appartient l'appelant.</summary>
        /// <returns>L'identifiant de l'organisation.</returns>
        Guid GetOrganizationId();
    }
}
