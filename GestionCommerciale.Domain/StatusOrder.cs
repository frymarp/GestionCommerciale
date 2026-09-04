using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Les états possibles d'une commande, dans l'ordre logique de son cycle de vie.
    /// </summary>
    public enum OrderStatus { Draft, Approved, Invoiced, Paid, Canceled }

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
        public static bool CanTransitionTo(OrderStatus current, OrderStatus target) =>
          (current, target) switch
         {
             (OrderStatus.Draft, OrderStatus.Approved) => true,
             (OrderStatus.Approved, OrderStatus.Invoiced) => true,
             (OrderStatus.Invoiced, OrderStatus.Paid) => true,
             (OrderStatus.Approved, OrderStatus.Canceled) => true,
             (OrderStatus.Draft, OrderStatus.Canceled) => true,
             _ => false
         };
    }
}
