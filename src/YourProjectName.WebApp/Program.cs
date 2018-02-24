using Guru.AspNetCore;
using Guru.AspNetCore.Abstractions;
using Guru.DependencyInjection;
using Guru.DependencyInjection.Attributes;
using Guru.Executable.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace YourProjectName.WebApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Guru.Executable.ConsoleAppInstance.Default.Run(args);
        }

        [Injectable(typeof(IConsoleExecutable), Lifetime.Singleton)]
        public class Startup : IConsoleExecutable
        {
            public int Run(string[] args)
            {
                new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls(DependencyContainer.Resolve<IApplicationConfiguration>()?.Urls ?? new string[] { "http://localhost:5000" })
                    .ConfigureServices(x => x.AddSingleton<IHttpContextAccessor, HttpContextAccessor>())
                    .Configure(x =>
                    {
                        HttpContextUtils.Configure(x.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
                        x.UseMiddleware<AspNetCoreAppInstance>();
                    })
                    .Build()
                    .Run();

                return 0;
            }

            public Task<int> RunAsync(string[] args)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}