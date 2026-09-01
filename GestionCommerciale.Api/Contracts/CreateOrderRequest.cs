namespace GestionCommerciale.Api.Contracts
{
    /// <summary>
    /// DTO (Data Transfer Object) : ce que l'API attend en entrée pour créer une commande et une ligne de commande.
    /// Volontairement différent de Order et OrderLine (l'entité du Domain) : pas de Guid (généré côté serveur),
    /// pas d'objet Email déjà validé — juste du texte brut, ce que le client HTTP peut réellement
    /// envoyer. C'est ce type que CreateOrderRequestValidator valide, et que l'endpoint transforme
    /// ensuite en vrai Order du Domain.
    /// </summary>
    public record CreateOrderRequest(Guid ClientId, List<OrderLineRequest> Lines);
    public record OrderLineRequest(Guid ProductId, int Quantity);
}
