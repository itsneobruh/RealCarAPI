using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RealCarAPI.Models
{
    public class RealCarAPIContext : DbContext
    {
        public RealCarAPIContext (DbContextOptions<RealCarAPIContext> options)
            : base(options)
        {
        }

        public DbSet<RealCarAPI.Models.CarItem> CarItem { get; set; }
    }
}
