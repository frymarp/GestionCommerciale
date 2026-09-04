namespace GestionCommerciale.Api.Contracts
{
    // DTO (Data Transfer Object) : ce que l'API attend en entrée pour créer une commande et une ligne de commande.
    // Volontairement différent de Order et OrderLine (l'entité du Domain) : pas de Guid (généré côté serveur),
    // pas d'objet Email déjà validé — juste du texte brut, ce que le client HTTP peut réellement
    // envoyer. C'est ce type que CreateOrderRequestValidator valide, et que l'endpoint transforme
    // ensuite en vrai Order du Domain.
    /// <summary>Requête de création d'une commande.</summary>
    /// <param name="ClientId">Id du client.</param>
    /// <param name="Lines">Lignes de commandes associées.</param>
    public record CreateOrderRequest(Guid ClientId, List<OrderLineRequest> Lines);
    /// <summary>Requête de création d'une ligne de commande.</summary>
    /// <param name="ProductId">Id du produit.</param>
    /// <param name="Quantity">Quantité du produit.</param>
    public record OrderLineRequest(Guid ProductId, int Quantity);
}
