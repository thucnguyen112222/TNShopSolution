using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNShopSulotion.Data.EntityFramework
{
    class TNShopContextFactory : IDesignTimeDbContextFactory<TNShopdbContext>
    {
        public TNShopdbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TNShopdbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-7EUG64E;Database=TNShop;Trusted_Connection=True;");
            return new TNShopdbContext(optionsBuilder.Options);
        }
    }
}
