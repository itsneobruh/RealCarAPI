using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RealCarAPI.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RealCarAPIContext(
                serviceProvider.GetRequiredService<DbContextOptions<RealCarAPIContext>>()))
            {
                // Look for any movies.
                if (context.CarItem.Count() > 0)
                {
                    return;   // DB has been seeded
                }

                context.CarItem.AddRange(
                    new CarItem
                    {
                        Title = "Evo 3",
                        Url = "https://thecarblob.blob.core.windows.net/images/Evo 3.jpg",
                        Tags = "Mitsubishi",  
                        Width = "100",
                        Height = "100",
                        Engine = "2000cc",
                        Cylinders = 4
                    }

                );
                context.SaveChanges();
            }
        }
    }
}
