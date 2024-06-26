﻿using Fiap.McTech.Domain.Entities.Clients;

namespace Fiap.McTech.Domain.Entities.Cart
{
    /// <summary>
    /// Represents a client's shopping cart in the system.
    /// </summary>
    public class CartClient : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartClient"/> class.
        /// </summary>
        public CartClient() { }

        /// <summary>
        /// Gets or sets the unique identifier of the client associated with the cart.
        /// </summary>
        public Guid? ClientId { get; internal set; }

        /// <summary>
        /// Gets or sets the client associated with the cart.
        /// </summary>
        public Client? Client { get; internal set; }

        /// <summary>
        /// Gets or sets the total value of all items in the cart.
        /// </summary>
        public decimal AllValue { get; internal set; } = 0;

        /// <summary>
        /// Gets or sets the list of items in the cart.
        /// </summary>
        public List<CartItem> Items { get; internal set; } = new List<CartItem>();

        /// <summary>
        /// Determines whether the cart is valid.
        /// </summary>
        /// <returns>True if the cart is valid; otherwise, false.</returns>
        public override bool IsValid()
        {
            return Id != Guid.Empty
                && Items != null && Items.Count > 0 && Items.All(i => i.IsValid());
        }

        /// <summary>
        /// Calculates the total value of all items in the cart.
        /// </summary>
        public void CalculateValueCart()
        {
            AllValue = Items.Sum(p => p.CalculateValue());
        }
    }
}
