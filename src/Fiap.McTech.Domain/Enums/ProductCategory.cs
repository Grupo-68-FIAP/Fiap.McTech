using System.ComponentModel;

namespace Fiap.McTech.Domain.Enums
{
	public enum ProductCategory
	{
		[Description("Nenhum")]
		None = -1,

		[Description("Lanche")]
		Snack = 0,

		[Description("Acompanhamento")]
		SideDish = 1,

		[Description("Bebida")]
		Beverage = 2,

		[Description("Sobremesa")]
		Dessert = 3
	}
}