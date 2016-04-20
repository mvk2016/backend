using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Entity;
using api.Interfaces;
using api.Models;
using api.Services;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Diagnostics.Entity;
using System.Net.WebSockets;
using api.Lib;

namespace api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            builder.AddEnvironmentVariables();
            Configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Use MVC
            services.AddMvc();

            // To connect to SQL Server, create a static class Config.cs with a
            // ADO.NET connection string called SqlServerConnectionString
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApiContext>(options => options.UseSqlServer(Config.SqlServerConnectionString));

            // Initialize Event Hub Connector as a singleton
            services.AddInstance<IEventHubConnector>(new EventHubConnector());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseStaticFiles();

            app.UseWebSockets();
            app.Use(async (http, next) =>
            {
                if (http.WebSockets.IsWebSocketRequest)
                {
                    // Websocket request, get socket and add to handler.
                    var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                    if (webSocket != null && webSocket.State == WebSocketState.Open)
                    {
                        WebSocketHandler.Add(webSocket);
                    }
                }
                else
                {
                    // Not a websocket request, let api handle request
                    await next();
                }
            });

            app.UseMvc();
            app.UseDatabaseErrorPage();
            app.UseDeveloperExceptionPage();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
