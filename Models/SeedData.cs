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
                        Title = "Is Mayo an Instrument?",
                        Url = "https://i.kym-cdn.com/photos/images/original/001/371/723/be6.jpg",
                        Tags = "spongebob",  
                        Width = "768",
                        Height = "432",
                        Engine = "1600cc",
                        Cylinders = 4
                    }


                );
                context.SaveChanges();
            }
        }
    }
}
