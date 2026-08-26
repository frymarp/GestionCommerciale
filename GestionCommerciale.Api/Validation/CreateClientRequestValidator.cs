using FluentValidation;
using GestionCommerciale.Api.Contracts;

namespace GestionCommerciale.Api.Validation
{
    /// <summary>
    /// Regroupe toutes les règles de validation de CreateClientRequest à un seul endroit, plutôt
    /// que des "if" éparpillés dans l'endpoint. Exécuté automatiquement par ValidationFilter&lt;T&gt;
    /// avant que le code de l'endpoint ne s'exécute.
    /// </summary>
    public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
    {
        public CreateClientRequestValidator()
        {
            // RuleFor(...) désigne la propriété concernée ; chaque méthode enchaînée après (NotEmpty,
            // EmailAddress, WithMessage...) ajoute une contrainte — c'est le style "fluent" qui donne
            // son nom à la bibliothèque.
            RuleFor(x => x.Name).NotEmpty().WithMessage("Le nom est obligatoire.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("L'email est invalide.");
        }
    }
}
