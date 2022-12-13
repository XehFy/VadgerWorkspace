using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Data.Entities;

namespace VadgerWorkspace.Data.Repositories
{
    public class MessageRepository : BaseRepository<SavedMessage>
    {
        public MessageRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
