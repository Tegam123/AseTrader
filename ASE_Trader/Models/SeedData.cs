using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASE_Trader.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ASE_Trader.Models.EntityModels
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.Users.Any())
                {
                    return;   // DB has been seeded
                }

                context.Users.AddRange(new User
                    {
                        FirstName = "David",
                        LastName = "Tegam",
                        Email = "dt@gmail.com",
                        PwHash = "1234"
                    },

                    new User
                    {
                        FirstName = "Victor",
                        LastName = "Kildahl",
                        Email = "vk@gmail.com",
                        PwHash = "1234"
                    },

                      new User
                      {
                          FirstName = "Lasse",
                          LastName = "Mosel",
                          Email = "lm@gmail.com",
                          PwHash = "1234"
                      }
                );
                context.SaveChanges();
            }
        }
    }
}

