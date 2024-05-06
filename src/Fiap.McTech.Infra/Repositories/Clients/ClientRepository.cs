using Fiap.McTech.Domain.Entities.Clients; 
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Fiap.McTech.Infra.Context;

namespace Fiap.McTech.Infra.Repositories.Clients
{
	public class ClientRepository : RepositoryBase<Client>, IClientRepository
	{
		public ClientRepository(DataContext context) : base(context) { }
	}
}
