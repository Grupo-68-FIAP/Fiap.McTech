﻿using Fiap.McTech.Domain.ValuesObjects;

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

		public string Name { get; private set; } = string.Empty;
		public Cpf Cpf { get; private set; } 
		public Email Email { get; private set; }

		public override bool IsValid()
		{
			return !string.IsNullOrEmpty(Name) &&
				   Cpf.IsValid() &&
				   Email.IsValid();
		}
	}
}