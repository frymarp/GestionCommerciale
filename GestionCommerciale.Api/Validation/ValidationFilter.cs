using FluentValidation;

namespace GestionCommerciale.Api.Validation
{
    /// <summary>
    /// Filtre d'endpoint générique et réutilisable : quel que soit T (CreateClientRequest,
    /// CreateOrderRequest...), il sait exécuter le bon validateur FluentValidation, à condition
    /// qu'un IValidator&lt;T&gt; correspondant soit enregistré dans le conteneur DI (fait
    /// automatiquement par AddValidatorsFromAssemblyContaining dans Program.cs).
    /// Appliqué à un endpoint via .AddEndpointFilter&lt;ValidationFilter&lt;CreateClientRequest&gt;&gt;().
    /// </summary>
    public class ValidationFilter<T> : IEndpointFilter
    {
        // Résolu automatiquement par DI grâce au constructeur : pas besoin de faire
        // new ValidationFilter<T>(...) soi-même.
        private readonly IValidator<T> _validator;

        /// <param name="validator">Le validateur FluentValidation correspondant au type T, fourni par le conteneur DI.</param>
        public ValidationFilter(IValidator<T> validator) => _validator = validator;

        /// <summary>
        /// Appelé automatiquement par ASP.NET Core avant le handler de l'endpoint.
        /// </summary>
        /// <param name="context">Contexte de la requête en cours, donnant accès à ses arguments (dont celui de type T).</param>
        /// <param name="next">Délégué représentant la suite du pipeline (le handler de l'endpoint, ou le filtre suivant).</param>
        /// <returns>
        /// Le résultat de next(context) si la validation réussit ; sinon un IResult de type
        /// ValidationProblem (400) contenant le détail des erreurs, sans jamais appeler next.
        /// </returns>
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            // Récupère l'argument de type T passé à l'endpoint (ex. le CreateClientRequest désérialisé
            // depuis le corps de la requête HTTP).
            var request = context.GetArgument<T>(0);
            var result = await _validator.ValidateAsync(request);

            if (!result.IsValid)
                // Court-circuite : le code de l'endpoint (next) n'est jamais appelé, le client reçoit
                // directement un 400 ProblemDetails avec le détail des erreurs par champ.
                return Results.ValidationProblem(result.ToDictionary());

            // Tout est valide : on laisse passer la requête vers le vrai code de l'endpoint.
            return await next(context);
        }
    }
}
