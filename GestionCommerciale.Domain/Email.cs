using System;
using System.Collections.Generic;
using System.Text;

namespace GestionCommerciale.Domain
{
    public record Email
    {
        /// <summary>
        /// Email address
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// Email constructor
        /// </summary>
        /// <param name="value">Email address (text, needs an '@')</param>
        /// <exception cref="ArgumentException">The email address is invalid.</exception>
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
                throw new ArgumentException("Invalid email", nameof(value));
            Value = value;
        }
    }

}
