using System;

namespace Fiap.McTech.Domain.Entities
{
	public abstract class EntityBase
	{
		public Guid Id { get; private set; } = Guid.NewGuid();
		public DateTime CreatedDate { get; private set; } = DateTime.Now;
		public DateTime UpdatedDate { get; set; } = DateTime.Now;

		public abstract bool IsValid();
	}
}