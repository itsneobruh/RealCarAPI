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
                        Url = "https://thecarblob.blob.core.windows.net/images/Evo%203.jpg?sv=2017-11-09&ss=bqtf&srt=sco&sp=rwdlacup&se=2018-11-22T19:50:23Z&sig=lxLiKdUDmc668QVXT7TnTihWu4SF2JC4OkNNHLxD%2BcA%3D&_=1542887426900",
                        Tags = "Mitsubishi",  
                        Width = "70",
                        Height = "43",
                        Engine = "2000cc",
                        Cylinders = 4
                    }

                );
                context.SaveChanges();
            }
        }
    }
}
