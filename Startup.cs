using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SecuLink.Services;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SecuLink
{
    public class Startup
    {
        private readonly string Name = "React";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<Models.ApplicationDbContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: Name,
                                  policy =>
                                  {
                                      policy.SetIsOriginAllowed(MyIsOriginAllowed)
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                      static bool MyIsOriginAllowed(string origin)
                                      {
                                          if (origin == "http://localhost:3000")
                                              return true;

                                          string localIP = GetLocalIPAddress();
                                          string networkID = "";
                                          int byteCount = 0;
                                          for (int i = 0; i < localIP.Length; i++)
                                          {
                                              networkID += localIP[i].ToString();
                                              if (localIP[i].ToString() == ".")
                                                  byteCount++;
                                              if (byteCount == 3)
                                                  break;
                                          }

                                          for (int i = 1; i < 255; i++)
                                          {
                                              if (origin == "http://" +  networkID + i.ToString())
                                                  return true;
                                          }
                                          for (int i = 1; i < 255; i++)
                                          {
                                              if (origin == "https://" + networkID + i.ToString())
                                                  return true;
                                          }

                                          return false;
                                      }
                                      static string GetLocalIPAddress()
                                      {
                                          var host = Dns.GetHostEntry(Dns.GetHostName());
                                          foreach (var ip in host.AddressList)
                                          {
                                              string firstByte = ip.ToString()[0].ToString() + ip.ToString()[1].ToString() + ip.ToString()[2].ToString();
                                              if (ip.AddressFamily == AddressFamily.InterNetwork && firstByte == "192")
                                              {
                                                  return ip.ToString();
                                              }
                                          }
                                          return "";
                                      }
                                  });
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(Name);

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "SecuLink",
                routeTemplate: "api/{controller}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
