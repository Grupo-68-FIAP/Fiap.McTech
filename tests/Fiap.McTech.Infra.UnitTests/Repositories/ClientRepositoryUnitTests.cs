using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Infra.Repositories.Clients;

namespace Fiap.McTech.Infra.UnitTests.Repositories
{
    public sealed class ClientRepositoryUnitTests : RepositoryBaseUnitTests<Client>
    {
        readonly static Client CLIENT_1 = new("Client 1", new("79703017037"), new("test1@test.com"));
        readonly static Client CLIENT_2 = new("Client 2", new("29255133012"), new("test2@test.com"));
        readonly static Client CLIENT_3 = new("Client 3", new("32161895036"), new("test3@test.com"));

        public ClientRepositoryUnitTests() : base(CLIENT_1)
        {
            // Popule o banco de dados em memória com alguns dados de teste
            _context.Clients?.Add(CLIENT_1);
            _context.Clients?.Add(CLIENT_2);
            _context.SaveChanges();
        }

        protected override ClientRepository GetRepository()
        {
            return new ClientRepository(_context);
        }

        protected override Client GetEntityToAddTest()
        {
            return CLIENT_3;
        }
    }
}
