using System;
using Brief.Areas.Identity.Data;
using Brief.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Brief.Areas.Identity.IdentityHostingStartup))]
namespace Brief.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<BriefContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("BriefContextConnection")));

                services.AddIdentityCore<BriefUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<BriefContext>();
            });
        }
    }
}