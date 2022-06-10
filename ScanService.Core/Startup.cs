using Microsoft.Extensions.DependencyInjection;
using ScanService.Core.Scanners.Services;
using ScanService.Core.Tasks;

namespace ScanService.Core
{
    public static class Startup
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ITaskProvider, TaskProvider>();
            services.AddScoped<IDirectoryScanner, DirectoryScanner>();
            
            return services;
        }
    }
}