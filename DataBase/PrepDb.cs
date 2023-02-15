using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PriceService.SignalR;

namespace PriceService.DataBase
{
    public static class PrepDb
    {
        public static void UpdateDB(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                var clientHub = serviceScope.ServiceProvider.GetService<ClientHub>();   
                clientHub.GetAllPlatforms();
            }
        }
    }
}