using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Les états possibles d'une commande, dans l'ordre logique de son cycle de vie.
    /// </summary>
    public enum StatusOrder { Draft, Approved, Invoiced, Paid, Canceled }

    /// <summary>
    /// Règle métier : quelles transitions de statut sont autorisées.
    /// (état de départ, état d'arrivée) => autorisé ou non
    /// </summary>
    public static class OrderTransitions
    {
        /// <summary>
        /// Transitions actuellement autorisées :
        /// Draft → Approved → Invoiced → Paid
        /// Draft → Canceled
        /// Approved → Canceled
        /// </summary>
        /// <param name="current">Statut actuel de la commande.</param>
        /// <param name="target">Statut vers lequel on souhaite passer.</param>
        /// <returns>true si le passage de current à target est autorisé, false sinon.</returns>
        public static bool CanTransitionTo(StatusOrder current, StatusOrder target) =>
          (current, target) switch
         {
             (StatusOrder.Draft, StatusOrder.Approved) => true,
             (StatusOrder.Approved, StatusOrder.Invoiced) => true,
             (StatusOrder.Invoiced, StatusOrder.Paid) => true,
             (StatusOrder.Approved, StatusOrder.Canceled) => true,
             (StatusOrder.Draft, StatusOrder.Canceled) => true,
             _ => false
         };
    }
}
