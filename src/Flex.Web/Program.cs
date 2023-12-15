using Autofac;
using Autofac.Extensions.DependencyInjection;
using Flex.Application.Extensions.AutoFacExtension;
using Flex.Application.Extensions.Jwt;
using Flex.Application.Extensions.Register.AutoMapper;
using Flex.Application.Extensions.Register.MemoryCacheExtension;
using Flex.Application.Extensions.Register.WebCoreExtensions;
using Flex.Application.Extensions.Swagger;
using Flex.Core.JsonConvertExtension;
using Flex.EFSqlServer;
using Flex.EFSqlServer.Register;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//注册swagger
builder.Services.AddSwagger(AssemblyName);
//注册视图控制器
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    // 在这里配置 JsonSerializerOptions
    options.JsonSerializerOptions.Converters.Add(new BooleanConverter());

    // 添加其他配置，如果有的话
}); ;
//注册EFcoreSqlserver
builder.Services.AddUnitOfWorkService<SqlServerContext>(item => item.UseSqlServer("DataConfig:Sqlserver:ConnectionString".Config(string.Empty)));

//注册Automapper
builder.Services.AddAutomapperService();
//跨域配置
builder.Services.AddCorsPolicy(builder.Configuration);
//注册autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).
    ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterAutoFacExtension();
        //注册业务层，同时对业务层的方法进行拦截
        builder.RegisterAssemblyTypes(Assembly.Load("Flex.Application"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    });

builder.Services.AddJwtService(builder.Configuration);
//注册webcore服务（网站主要配置）
builder.Services.AddWebCoreService(builder.Configuration);
//注册缓存
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
