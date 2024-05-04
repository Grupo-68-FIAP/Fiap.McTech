using Fiap.McTech.Domain.Utils.Extensions;

namespace Fiap.McTech.Domain.ValuesObjects
{
	public class Email : ValueObject
	{
        public string Address { get; set; }

        public Email(string email)
		{
			Address = email;
		}

		public override bool IsValid() => Address.IsValidEmail();
	}
}