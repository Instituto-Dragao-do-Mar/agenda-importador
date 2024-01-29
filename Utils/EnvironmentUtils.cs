using System;

namespace Domain.Utils
{
    public static class EnvironmentUtils
    {
        private static bool IsProduction => false;

        public static string GetEnvironment()
        {
            return IsProduction ? "Production" : "Development";
        }

        public static bool EnvIsProduction() => IsProduction;
    }
}