using Bpst.API.DbModels; 
using Microsoft.EntityFrameworkCore;

namespace Bpst.API.DB
{
    public static class ModelBuilderExtention
    {
        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role() { UniqueId = 1, RoleName = "SuperAdmin" },
                new Role() { UniqueId = 2, RoleName = "HR" },
                new Role() { UniqueId = 3, RoleName = "Dev" },
                new Role() { UniqueId = 4, RoleName = "Tranner" },
                new Role() { UniqueId = 5, RoleName = "Student" },
                new Role() { UniqueId = 6, RoleName = "AccountFees" },
                new Role() { UniqueId = 7, RoleName = "AccountSales" },
                new Role() { UniqueId = 8, RoleName = "AccountSalary" },
                new Role() { UniqueId = 9, RoleName = "AccountInvoicing" }
                );
        }
    }
}
