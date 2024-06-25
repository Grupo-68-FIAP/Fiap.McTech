using System.ComponentModel;

namespace Fiap.McTech.Domain.Enums
{
    /// <summary>
    /// Represents the category of a product:<br></br>
    /// <list type="bullet">
    /// <item><description><c>None</c> (-1): No specific category.</description></item>
    /// <item><description><c>Snack</c> (0): Category for snacks.</description></item>
    /// <item><description><c>SideDish</c> (1): Category for side dishes.</description></item>
    /// <item><description><c>Beverage</c> (2): Category for beverages.</description></item>
    /// <item><description><c>Dessert</c> (3): Category for desserts.</description></item>
    /// </list>
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
