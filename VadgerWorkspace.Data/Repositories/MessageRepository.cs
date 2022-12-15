using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Data.Entities;
using VadgerWorkspace.Data.Migrations;

namespace VadgerWorkspace.Data.Repositories
{
    public class MessageRepository : BaseRepository<SavedMessage>
    {
        public MessageRepository(DbContext dbContext) : base(dbContext)
        {
            
        }
        public async Task<IEnumerable<SavedMessage>> GetAllMessagesByClientId(long clientId)
        {
            return await Task.FromResult(FindByCondition(message => message.ClientId == clientId));
        }
    }
}
