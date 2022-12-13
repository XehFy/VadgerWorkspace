using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Data.Entities;

namespace VadgerWorkspace.Data
{
    public class VadgerContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<SavedMessage> Messages { get; set; }

        //public string DbPath { get; }

        public VadgerContext ()
        {
            //DbPath = "VadgerDb.db";
        }

        public VadgerContext(DbContextOptions<VadgerContext> options) : base(options) { }
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite($"Filename =C:/Users/renat/Desktop/VadgerWorkspace{DbPath}");
    }
}
