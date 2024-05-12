using System;
using System.Text.RegularExpressions;

namespace Fiap.McTech.Domain.Utils.Extensions
{
	public static class StringExtensions
	{
		public static bool IsValidCpf(this string cpf)
		{
			if (string.IsNullOrWhiteSpace(cpf))
				return false;

			try
			{
				// Remove caracteres não numéricos do CPF
				cpf = new string(cpf.Where(char.IsDigit).ToArray());

				if (cpf.Length != 11)
					return false;

				// Verifica se todos os dígitos são iguais, o que torna o CPF inválido
				if (cpf.Distinct().Count() == 1)
					return false;

				// Verifica os dígitos verificadores
				int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
				int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

				string tempCpf = cpf.Substring(0, 9);
				int sum = 0;

				for (int i = 0; i < 9; i++)
					sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

				int remainder = sum % 11;
				remainder = remainder < 2 ? 0 : 11 - remainder;

				string digit = remainder.ToString();
				tempCpf += digit;

				sum = 0;
				for (int i = 0; i < 10; i++)
					sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

				remainder = sum % 11;
				remainder = remainder < 2 ? 0 : 11 - remainder;

				digit += remainder.ToString();

				return cpf.EndsWith(digit);
			}
			catch
			{
				return false; 
			}
		}

		public static bool IsValidEmail(this string email)
		{
			if (string.IsNullOrWhiteSpace(email)) return false;

			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch (FormatException)
			{
				return false;
			}
		}
	}
}