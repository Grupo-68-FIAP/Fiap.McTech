using Fiap.McTech.Domain.ValuesObjects;

namespace Fiap.McTech.Domain.Entities.Clients
{
	public class Client : EntityBase
	{
        public Client(string name, Cpf cpf, Email email)
		{
			Name = name;
			Cpf = cpf;
			Email = email;
		}

		public string Name { get; internal set; } = string.Empty;
		public Cpf Cpf { get; internal set; } 
		public Email Email { get; internal set; }

		public override bool IsValid()
		{
			return !string.IsNullOrEmpty(Name)
				&& Cpf != null && Cpf.IsValid()
				&& Email != null && Email.IsValid();
		}
	}
}