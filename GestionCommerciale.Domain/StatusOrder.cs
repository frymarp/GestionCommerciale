using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Status order: Draft, Approved, Invoiced, Paid, Canceled 
    /// </summary>
    public enum StatusOrder { Draft, Approved, Invoiced, Paid, Canceled }

    /// <summary>
    /// Order transitions: Draft -> Approved -> Invoiced -> Paid/Canceled and Draft -> Canceled
    /// </summary>
    public static class OrderTransitions
    {
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
