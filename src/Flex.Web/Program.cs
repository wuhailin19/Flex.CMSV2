using Autofac;
using Autofac.Extensions.DependencyInjection;
using Flex.Application.Extensions.AutoFacExtension;
using Flex.Application.Extensions.Jwt;
using Flex.Application.Extensions.Register.AutoMapper;
using Flex.Application.Extensions.Register.MemoryCacheExtension;
using Flex.Application.Extensions.Register.WebCoreExtensions;
using Flex.Application.Extensions.Swagger;
using Flex.EFSqlServer;
using Flex.EFSqlServer.Register;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//×¢²áswagger
builder.Services.AddSwagger(AssemblyName);
//×¢²áÊÓÍ¼¿ØÖÆÆ÷
builder.Services.AddControllersWithViews();
//×¢²áEFcoreSqlserver
builder.Services.AddUnitOfWorkService<SqlServerContext>(item => item.UseSqlServer("DataConfig:Sqlserver:ConnectionString".Config(string.Empty)));

//×¢²áAutomapper
builder.Services.AddAutomapperService();
//¿çÓòÅäÖÃ
builder.Services.AddCorsPolicy(builder.Configuration);
//×¢²áautofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).
    ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterAutoFacExtension();
        //×¢²áÒµÎñ²ã£¬Í¬Ê±¶ÔÒµÎñ²ãµÄ·½·¨½øÐÐÀ¹½Ø
        builder.RegisterAssemblyTypes(Assembly.Load("Flex.Application"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    });

builder.Services.AddJwtService(builder.Configuration);
//×¢²áwebcore·þÎñ£¨ÍøÕ¾Ö÷ÒªÅäÖÃ£©
builder.Services.AddWebCoreService(builder.Configuration);
//×¢²á»º´æ
builder.Services.AddMemoryCacheSetup();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AssemblyName} v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.UseCors(WebCoreSetupExtension.MyAllowSpecificOrigins);
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "AreaRoute",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Login}/{action=Index}/{id?}");
});

app.Run();
