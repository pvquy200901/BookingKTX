using Microsoft.EntityFrameworkCore;
using Serilog;
using Newtonsoft.Json;
using System.Drawing.Drawing2D;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using System;
using BookingKTX.Models;
using BookingKTX.APIs;

namespace BookingKTX;

public class Program
{
    public static MyFile api_file = new MyFile();
    public static MyUser api_user = new MyUser();
    public static MyState api_state = new MyState();
    public static MyType api_type = new MyType();
    public static MyRole api_role = new MyRole();

    /*public static IHubContext<NotificationDeviceHub>? notificationDeviceHub;
    public static IHubContext<NotificationUserHub>? notificationUserHub;*/


    //public class HttpRequestUser
    //{
    //    public string id { get; set; } = "";
    //    public bool isOnline { get; set; } 
    //    public List<string> messagers { get; set; } = new List<string>();
    //}

    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
               .WriteTo.File("mylog.txt", rollingInterval: RollingInterval.Day)
               .CreateLogger();
        try
        {
          
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel((context, option) =>
            {
                option.ListenAnyIP(50000, listenOptions =>
                {
                   
                });

                option.ListenAnyIP(50001, listenOptions =>
                {
                });
                option.Limits.MaxConcurrentConnections = null;
                option.Limits.MaxRequestBodySize = null;
                option.Limits.MaxRequestBufferSize = null;
            });
            // Add services to the container.
            //builder.Logging.AddSerilog();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("HTTPSystem", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).WithExposedHeaders("Grpc-Status", "Grpc-Encoding", "Grpc-Accept-Encoding");
                });
            });


           /* builder.Services.AddSignalR(option =>
            {
                option.EnableDetailedErrors = true;
                option.KeepAliveInterval = TimeSpan.FromSeconds(5);
                option.MaximumReceiveMessageSize = 10 * 1024 * 1024;
                option.StreamBufferCapacity = 10 * 1024 * 1024;
            }).AddMessagePackProtocol();*/

            builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(DataContext.configSql));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                DataContext datacontext = services.GetRequiredService<DataContext>();
                datacontext.Database.EnsureCreated();
                await datacontext.Database.MigrateAsync();
            }

            Log.Information(String.Format("Connected to Server at : {0} with : {1} ", DateTime.Now, DataContext.configSql));

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();

            app.UseCors("HTTPSystem");
            app.UseRouting();

            app.UseAuthorization();

           /* app.UseEndpoints(endpoints =>
            {

                endpoints.MapHub<NotificationDeviceHub>("/notificationdevicehub", options =>
                {
                    options.Transports = HttpTransportType.WebSockets;
                });
                endpoints.MapHub<NotificationUserHub>("/notificationuserhub", options =>
                {
                    options.Transports = HttpTransportType.WebSockets;
                });
            });

            notificationDeviceHub = (IHubContext<NotificationDeviceHub>?)app.Services.GetService(typeof(IHubContext<NotificationDeviceHub>));
            notificationUserHub = (IHubContext<NotificationUserHub>?)app.Services.GetService(typeof(IHubContext<NotificationUserHub>));*/

            app.MapControllers();
            app.MapGet("/", () => string.Format("Booking KTX - {0}", DateTime.Now));

            await api_role.initAsync();
            await api_user.initAsync();
            //api_file.initCreateTargetFile("EXB_1");

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }

        Log.CloseAndFlush();
    }
}