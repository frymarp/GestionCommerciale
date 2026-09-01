using FluentValidation;
using GestionCommerciale.Api.Contracts;

namespace GestionCommerciale.Api.Validation
{
    /// <summary>
    /// Regroupe toutes les règles de validation de CreateProductRequest à un seul endroit, plutôt
    /// que des "if" éparpillés dans l'endpoint. Exécuté automatiquement par ValidationFilter<>
    /// avant que le code de l'endpoint ne s'exécute.
    /// </summary>
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Le nom est obligatoire.");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Le prix doit être supérieur ou égal à 0.");
        }
    }
}
