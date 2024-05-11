using Fiap.McTech.Domain.Entities.Clients; 
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Fiap.McTech.Domain.ValuesObjects;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.Repositories.Clients
{
	public class ClientRepository : RepositoryBase<Client>, IClientRepository
	{
		public ClientRepository(DataContext context) : base(context) { }

        public async Task<Client?> GetClientByCpfAsync(Cpf cpf)
        {
            return await _dbSet.Where(c => cpf.Equals(c.Cpf)).FirstOrDefaultAsync();
        }
    }
}
