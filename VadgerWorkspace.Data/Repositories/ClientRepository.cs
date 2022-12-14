using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Data.Entities;

namespace VadgerWorkspace.Data.Repositories
{
    public class ClientRepository : BaseRepository<Client>
    {
        public ClientRepository(DbContext dbContext) : base(dbContext)
        {
        }
        public void SaveSync()
        {
            _dbContext.SaveChanges();
        }

        public async Task<Client> GetClientByIdAsync(long Id)
        {
            return await FindByCondition(client => client.Id == Id).FirstOrDefaultAsync();
        }

        public Client GetClientByIdSync(long id)
        {
            return _dbContext.Set<Client>().FirstOrDefault(o => o.Id == id);
        }

        public async Task<IEnumerable<Client>> GetAllClientsForEmployee(long employeeId)
        {
            return await Task.FromResult(FindByCondition(client => client.EmployeeId == employeeId));
        }
    }
}
