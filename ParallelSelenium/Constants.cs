using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelSelenium
{
    public static class Constants
    {
        internal static string sauceUser = "asad360logica";
        internal static string sauceKey = "4b140c78-2f70-4e2b-a733-e556e02ecde9";
        internal static string tunnelId = Environment.GetEnvironmentVariable("TUNNEL_IDENTIFIER");
        internal static string seleniumRelayPort = Environment.GetEnvironmentVariable("SELENIUM_PORT");
        internal static string buildTag = "NUniT-Sample";
        internal static string seleniumRelayHost = Environment.GetEnvironmentVariable("SELENIUM_HOST");

    }
}