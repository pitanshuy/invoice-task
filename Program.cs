using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;


namespace invoice_task
{

    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        Log.Logger = new LoggerConfiguration()
    //            .WriteTo.Console() // Logs to the console
    //            .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day) // Logs to a file, rolling daily
    //            .CreateLogger();

    //        try
    //        {
    //            Log.Information("Starting web host...");
    //            CreateHostBuilder(args).Build().Run();
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Fatal(ex, "Application failed to start");
    //        }
    //        finally
    //        {
    //            Log.CloseAndFlush();
    //        }
    //    }

    //    public static IHostBuilder CreateHostBuilder(string[] args) =>
    //        Host.CreateDefaultBuilder(args)
    //            .ConfigureWebHostDefaults(webBuilder =>
    //            {
    //                webBuilder.UseStartup<Startup>()
    //                          .UseSerilog(); // Uses Serilog for logging
    //            });
    //}


    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog at the start
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting up the application...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // Add this line to use Serilog
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }



}
