using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VadgerWorkspace.Data.Entities
{
    public class Employee
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string? Name { get; set; }
        public string? Town { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsLocalAdmin { get; set; }
        public bool? IsVerified { get; set; }
        public Stages Stage { get; set; }
        public long? ManagementId { get; set; }
    }
}