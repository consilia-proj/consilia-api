using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ConsiliaAPI
{
    public static class ErrorReporter
    {
        public static async Task LogErrorAsync(HttpContext context, Exception exception)
        {
            // Implement your logging logic here
        }
    }
}