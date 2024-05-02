using System;

namespace Fiap.McTech.Domain.Entities.Payments
{
	public class Payment : EntityBase
	{
        public Guid ClientId { get; set; }

        public override bool IsValid()
		{
			throw new NotImplementedException();
		}
	}
}