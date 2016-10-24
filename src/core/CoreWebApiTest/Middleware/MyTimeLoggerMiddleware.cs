using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CoreWebApiTest.Middleware
{
    public class MyTimeLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public MyTimeLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();

            await _next(context);

            Console.WriteLine("Response generation length {0} msec", watch.ElapsedMilliseconds);
        }
    }
}
