using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VadgerWorkspace.Data.Entities
{
    public class Client
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Town { get; set; }
        public string? Service { get; set; }
        public long? Link { get; set; }
        public Stages? Stage { get; set; }
        public long? EmployeeId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastOrder { get; set; }
        public string? Tag { get; set; }
    }
}
