using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VadgerWorkspace.Data.Entities
{
    public class SavedMessage
    {
        public int Id { get; set; }
        public DateTime? Time { get; set; }
        public long? ClientId { get; set; }
        public long? EmployeeId { get; set; }
        public bool? IsFromClient { get; set; }
        public string? Text { get; set; }
    }
}
