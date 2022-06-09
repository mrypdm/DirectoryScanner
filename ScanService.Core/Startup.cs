using Microsoft.Extensions.DependencyInjection;
using ScanService.Core.Scanners.Services;

namespace ScanService.Core
{
    public static class Startup
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IDirectoryScanner, DirectoryScanner>();
            
            return services;
        }
    }
}