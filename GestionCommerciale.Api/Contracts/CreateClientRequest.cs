namespace GestionCommerciale.Api.Contracts
{
    // DTO (Data Transfer Object) : ce que l'API attend en entrée pour créer un client.
    // Volontairement différent de Client (l'entité du Domain) : pas de Guid (généré côté serveur),
    // pas d'objet Email déjà validé — juste du texte brut, ce que le client HTTP peut réellement
    // envoyer. C'est ce type que CreateClientRequestValidator valide, et que l'endpoint transforme
    // ensuite en vrai Client du Domain.
    /// <summary>Requête de création d'un client.</summary>
    /// <param name="Name">Nom complet du client.</param>
    /// <param name="Email">Adresse email du client, doit respecter le format standard.</param>
    public record CreateClientRequest(string Name, string Email);
}
