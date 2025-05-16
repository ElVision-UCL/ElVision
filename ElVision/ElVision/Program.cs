using ElVision.Components;
using ElVision.Components.Account;
using ElVision.Data;
using ElVision.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using ElVisionLibrary.Models.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using ElVision.Middleware;
using Azure.Identity;
using ApexCharts;
using System.Globalization;

namespace ElVision
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                var keyVaultUrl = $"https://{builder.Configuration["KeyVault:Name"]}{builder.Configuration["KeyVault:BaseUrl"]}";
                builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());

                builder.Services.AddOpenTelemetry()
                    .UseAzureMonitor(options =>
                    {
                        options.ConnectionString = builder.Configuration["ApplicationInsights"];
                        options.SamplingRatio = 1.0F;
                    }).WithTracing(tracing => tracing
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.IsDevelopment() ? "Local" : "Cloud"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation());

                var danishCulture = new CultureInfo("da-DK");
                CultureInfo.DefaultThreadCurrentCulture = danishCulture;
                CultureInfo.DefaultThreadCurrentUICulture = danishCulture;

                builder.Services.AddMudServices();

                builder.Services.AddRazorComponents()
                    .AddInteractiveServerComponents();

                builder.Services.AddApexCharts(options =>
                {
                    options.GlobalOptions = new ApexChartBaseOptions
                    {
                        Debug = true,
                        Theme = new Theme
                        {
                            Mode = Mode.Dark
                        },
                        Colors = new List<string> { "#2F4F4F", "#1E90FF", "#FF6347", "#FFD700", "#8B0000", "#32CD32", "#FF4500", "#800080", "#FF1493", "#228B22", "#A52A2A" },

                        // limit number of tooltip to 2 decimals
                        Tooltip = new Tooltip
                        {
                            Y = new TooltipY
                            {
                                Formatter = @"function (val) 
                                {
                                    return val.toFixed(2) + ' kWh';
                                }"
                            }
                        }
                    };
                });

                builder.Services.AddCascadingAuthenticationState();
                builder.Services.AddScoped<IdentityUserAccessor>();
                builder.Services.AddScoped<IdentityRedirectManager>();
                builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
                builder.Services.AddSingleton<APIEncryption>();
                APIEncryption.SetConfiguration(builder.Configuration);

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                }).AddIdentityCookies();

                builder.Services.AddDbContext<UserDbContext>(options =>
                    options.UseMySql(builder.Configuration["IdentityConnection"],
                    new MySqlServerVersion(new Version(8, 0, 21))));

                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<UserDbContext>()
                    .AddSignInManager()
                    .AddDefaultTokenProviders();

                builder.Services.AddHttpClient();
                builder.Services.AddScoped<ILoadingService, LoadingService>();
                builder.Services.AddScoped<INotificationService, NotificationService>();
                builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();
                builder.Services.AddSingleton<IElOverblikService, ElOverblikService>();
                builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
                builder.Services.AddScoped<ITariffsService, TariffsService>();
                builder.Services.AddSingleton<IElspotService, ElspotService>();
                builder.Services.AddSingleton<IClimateReportService, ClimateReportService>();
                builder.Services.AddHostedService<ClimateReportBackgroundService>();

                var app = builder.Build();

                app.Logger.LogInformation("Application is starting...");

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseMiddleware<RedirectRootMiddleware>();

                app.UseHttpsRedirection();

                app.UseStaticFiles();
                app.UseAntiforgery();

                app.MapRazorComponents<App>()
                    .AddInteractiveServerRenderMode();

                // Add additional endpoints required by the Identity /Account Razor components.
                app.MapAdditionalIdentityEndpoints();

                app.Lifetime.ApplicationStopping.Register(() =>
                {
                    app.Logger.LogInformation("Application is stopping...");
                    // Add any additional cleanup logic here
                });

                app.Lifetime.ApplicationStopped.Register(() =>
                {
                    app.Logger.LogInformation("Application stopped.");
                    // Add any final cleanup logic here
                });

                app.Run();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Application failed to start: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace);

                // Use Serilog instead of Console for better tracking for example:
                // Log.Logger.Fatal(ex, "Application terminated unexpectedly");

                Environment.Exit(1);
            }
        }
    }
}
