using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Exceptions;

namespace Fiap.McTech.Domain.Entities.Cart
{
    /// <summary>
    /// Represents a client's shopping cart in the system.
    /// </summary>
    public partial class CartClient : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartClient"/> class with specified parameters.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client associated with the cart.</param>
        /// <param name="allValue">The total value of all items in the cart.</param>
        public CartClient(Guid? clientId, decimal allValue)
        {
            ClientId = clientId;
            AllValue = allValue;
            Items = new();
        }

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
        public List<Item> Items { get; internal set; }

        /// <summary>
        /// Determines whether the cart is valid.
        /// </summary>
        /// <returns>True if the cart is valid; otherwise, false.</returns>
        public override bool IsValid()
        {
            return Id != Guid.Empty
                && Items != null && Items.Count > 0 && Items.TrueForAll(i => i.IsValid());
        }

        /// <summary>
        /// Calculates the total value of all items in the cart.
        /// </summary>
        public void CalculateValueCart()
        {
            AllValue = Items.Sum(p => p.CalculateValue());
        }

        /// <summary>
        /// Adds a product to the cart.
        /// </summary>
        /// <param name="product"> Product to be added to the cart. </param>
        /// <param name="quantity"> Quantity of the product to be added to the cart. </param>
        public void AddProduct(Product product, int quantity)
        {
            var item = Items.Find(i => i.ProductId == product.Id);

            if (item == null && quantity > 0)
            {
                Items.Add(new(product.Name, quantity, product.Value, product.Id, this.Id));
            }
            else if (item != null && (item.Quantity + quantity) > 0)
            {
                item.Quantity += quantity;
            }
            else if (item != null && (item.Quantity + quantity) == 0)
            {
                Items.Remove(item);
            }
            else
            {
                throw new EntityValidationException("Invalid quantity.");
            }

            CalculateValueCart();
        }

        /// <summary>
        /// Removes a product from the cart.
        /// </summary>
        /// <param name="productId">Identifier of the product to be removed from the cart.</param>
        public void RemoveProduct(Guid productId)
        {
            var item = Items.Find(i => i.ProductId == productId)
                ?? throw new EntityNotFoundException("Product not found in the cart.");

            Items.Remove(item);
            CalculateValueCart();
        }
    }
}
