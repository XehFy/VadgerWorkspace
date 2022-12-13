﻿using System;
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
        public byte? RegistrationStage { get; set; }
        public long? EmployeeId { get; set; }
    }
}