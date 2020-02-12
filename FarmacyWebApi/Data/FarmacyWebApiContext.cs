using FarmacyWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmacyWebApi.Data
{
    public class FarmacyWebApiContext : DbContext
    {
        public FarmacyWebApiContext(DbContextOptions<FarmacyWebApiContext> options) : base(options)
        {
           Database.EnsureCreated();
        }
        public DbSet<User> User { get; set; }
    }
}
