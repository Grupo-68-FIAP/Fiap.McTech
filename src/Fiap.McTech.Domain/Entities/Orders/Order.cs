using System;

namespace Fiap.McTech.Domain.Entities.Orders
{
	public class Order : EntityBase
	{
        public int MyProperty { get; private set; }

		public override bool IsValid()
		{
			throw new NotImplementedException();
		}
	}
}