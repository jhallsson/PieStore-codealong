using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace codealong_pie_project
{
    public class Program
    {
        //service - an electric, gas, hybrid, or diesel car
        //client - a driver who uses the car the same way regardless of the engine
        //interface - automatic, ensures driver does not have to understand engine details like gears
        //injector - the parent who bought the kid the car and decided which kind
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
