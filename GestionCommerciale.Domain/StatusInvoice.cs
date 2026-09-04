using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Les états possibles d'une facture.
    /// </summary>
    public enum InvoiceStatus { Pending, Paid,Canceled }

    /// <summary>
    /// Règle métier : quelles transitions de statut sont autorisées.
    /// (état de départ, état d'arrivée) => autorisé ou non
    /// </summary>
    public static class InvoiceTransitions
    {
        /// <summary>
        /// Transitions actuellement autorisées :
        /// Pending → Paid
        /// Pending → Canceled
        /// </summary>
        /// <param name="current">Statut actuel de la facture.</param>
        /// <param name="target">Statut vers lequel on souhaite passer.</param>
        /// <returns>true si le passage de current à target est autorisé, false sinon.</returns>
        public static bool CanTransitionTo(InvoiceStatus current, InvoiceStatus target) =>
          (current, target) switch
          {
              (InvoiceStatus.Pending, InvoiceStatus.Paid) => true,
              (InvoiceStatus.Pending, InvoiceStatus.Canceled) => true,
              _ => false
          };
    }
}
