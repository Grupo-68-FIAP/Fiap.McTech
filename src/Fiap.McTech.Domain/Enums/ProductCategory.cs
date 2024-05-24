using System.ComponentModel;

namespace Fiap.McTech.Domain.Enums
{
    /// <summary>
    /// Represents the category of a product.
    /// </summary>
    public enum ProductCategory
    {
        /// <summary>
        /// No category.
        /// </summary>
        [Description("Nenhum")]
        None = -1,

        /// <summary>
        /// The product is a snack.
        /// </summary>
        [Description("Lanche")]
        Snack = 0,

        /// <summary>
        /// The product is a side dish.
        /// </summary>
        [Description("Acompanhamento")]
        SideDish = 1,

        /// <summary>
        /// The product is a beverage.
        /// </summary>
        [Description("Bebida")]
        Beverage = 2,

        /// <summary>
        /// The product is a dessert.
        /// </summary>
        [Description("Sobremesa")]
        Dessert = 3
    }
}