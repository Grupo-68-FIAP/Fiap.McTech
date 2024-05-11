using Fiap.McTech.Domain.Utils.Extensions;

namespace Fiap.McTech.Domain.ValuesObjects
{
	public class Cpf : ValueObject
	{
		public Cpf(string document)
		{
			Document = document;
		}

		public string Document { get; set; } = string.Empty;

		public override bool IsValid() => Document.IsValidCpf();

		public override string ToString() => IsValid() ? Document : "";
	}
}