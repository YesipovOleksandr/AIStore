using Microsoft.AspNetCore.Localization;
using AIStore.Dependencies;
using AIStore.Web.MappingProfile;
using AIStore.Web.Providers;
using AIStore.Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using AIStore.DAL.Context;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.RegisterDependencyModules();

builder.Services.AddAutoMapper(typeof(WebMappingProfile));

builder.Services.MapSettings(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseHttpsRedirection();

var staticFilesConfigurator = app.Services.GetService<IStaticFilesConfigurator>();
staticFilesConfigurator.ConfigureStaticPaths(app, app.Environment);

app.UseRouting();

var cultures = RouteDataRequestCultureProvider.GetCultures();
var options = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = cultures,
    SupportedUICultures = cultures,
};

options.RequestCultureProviders = new[]
{
      new RouteDataRequestCultureProvider { Options = options }
 };
app.UseRequestLocalization(options);

app.UseAuthorization();

var routesConfigurator = app.Services.GetService<IRoutesConfigurator>();

app.UseEndpoints(routesConfigurator.BuildRoutesUsingAIStores);

app.Run();

