using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    /// <summary>
    /// Historisation de la facture, étape après passage de pending à cancelled or paid.
    /// </summary>
    /// <param name="InvoiceId">Identifiant unique du client.</param>
    /// <param name="Description">Description</param>
    /// <param name="createdDate">Date de création</param>
    public record InvoiceHistory(Guid InvoiceId, string Description, DateTime createdDate)
    {
    }

}
