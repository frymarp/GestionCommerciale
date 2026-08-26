using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Value object Email.
    /// Contrairement à Client (une entité, identifiée par son Id), un value object n'a pas d'identité
    /// propre, il est défini entièrement par sa valeur. Deux Email avec la même chaîne sont égaux.
    /// Son rôle est de garantir qu'un email invalide ne peut tout simplement pas exister en mémoire :
    /// impossible de construire un Email "cassé", l'exception part dès le constructeur.
    /// </summary>
    public record Email
    {
        /// <summary>
        /// La valeur brute de l'email, en lecture seule une fois l'objet construit (pas de setter).
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Construit un Email, en validant le format au passage.
        /// </summary>
        /// <param name="value">L'email en texte brut ; doit être non vide et contenir un '@'.</param>
        /// <exception cref="ArgumentException">Levée si l'email est vide ou ne contient pas de '@'.</exception>
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
                throw new ArgumentException("Invalid email", nameof(value));
            Value = value;
        }
    }

}
