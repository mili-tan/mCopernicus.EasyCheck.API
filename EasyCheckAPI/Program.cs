using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Nancy;
using Nancy.Hosting.Self;
using mCopernicus.EasyChecker;

namespace EasyCheckAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            using (NancyHost host = new NancyHost(new HostConfiguration
                    { RewriteLocalhost = true, UrlReservations = new UrlReservations { CreateAutomatically = true } },
                new Uri("http://localhost:2020/")))
            {
                host.Start();

                Console.WriteLine("EasyCheck is running on 2020 port");
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
            }
        }
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/",
                x => Response.AsText("{\"owner\": \"Milkey\",\"message\": \"Hello,World!\"}", "application/json"));

            Get("/{host}/{port}",
                x =>
                {
                    List<int> list = MPing.Tcping(x.host.ToString(), Convert.ToInt32(x.port.ToString()));
                    return Response.AsText(
                        $"{{\"status\": {(list.Max() != 0).ToString().ToLower()},\"time\": {list.Average()}}}",
                        "application/json");
                });
        }
    }
}
