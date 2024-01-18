using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Flex.Application.Aop;
using Flex.Application.Exceptions;
using Flex.Application.Extensions.AutoFacExtension;
using Flex.Application.Extensions.Register.AutoMapper;
using Flex.Application.Extensions.Register.MemoryCacheExtension;
using Flex.Application.Extensions.Register.WebCoreExtensions;
using Flex.Application.Extensions.Swagger;
using Flex.Application.SetupExtensions;
using Flex.Dapper;
using Flex.EFSqlServer;
using Flex.EFSqlServer.Register;
using Flex.Web.Jwt;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NLog.Web;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add(typeof(GlobalExceptionFilter)));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//注册swagger
builder.Services.AddSwagger(AssemblyName);
//注册视图控制器
builder.Services.AddControllersWithViews();
//注册EFcoreSqlserver
builder.Services.AddUnitOfWorkService<SqlServerContext>(item => item.UseSqlServer("DataConfig:Sqlserver:ConnectionString".Config(string.Empty)));

builder.Services.AddSingleton<MyDBContext>();

//注册Automapper
builder.Services.AddAutomapperService();
//跨域配置
builder.Services.AddCorsPolicy(builder.Configuration);

builder.Services.HtmlTemplateDictInit();
//注册autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).
    ConfigureContainer<ContainerBuilder>(builder =>
    {
        //注册拦截器，同步异步都要
        builder.RegisterType<LogInterceptor>().AsSelf();
        builder.RegisterType<LogInterceptorAsync>().AsSelf();
        builder.RegisterAutoFacExtension();
        //注册业务层，同时对业务层的方法进行拦截
        builder.RegisterAssemblyTypes(Assembly.Load("Flex.Application"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;// 注册被拦截的类并启用类拦截
            .InterceptedBy(typeof(LogInterceptor));//这里只有同步的，因为异步方法拦截器还是先走同步拦截器 
    });

builder.Services.AddJwtService(builder.Configuration);
//注册webcore服务（网站主要配置）
builder.Services.AddWebCoreService(builder.Configuration);
//注册缓存
builder.Services.AddMemoryCacheSetup();

builder.Host.UseNLog();

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
////配置新的文件夹白名单
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.WebRootPath, "filemanage")),
//    RequestPath = "/filemanage"
//});

//FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
//// add new mapping.
//provider.Mappings[".myapp"] = "application/x-msdownload";
//provider.Mappings[".htm3"] = "text/html";
//provider.Mappings[".gitignore"] = "text/html";
//provider.Mappings[".yml"] = "text/html";
//provider.Mappings[".config"] = "text/html";
//provider.Mappings[".image"] = "image/png";
//provider.Mappings[".png"] = "image/png";

//app.UseStaticFiles(new StaticFileOptions()
//{
//    FileProvider = new PhysicalFileProvider(@"E:\"),  // 指定静态文件目录
//    RequestPath = "/server",
//    ServeUnknownFileTypes = true,
//    ContentTypeProvider = provider,
//    DefaultContentType = "application/x-msdownload", // 设置未识别的MIME类型一个默认z值
//});
//// replace an existing mepping.

//app.UseDirectoryBrowser();
//app.UseDirectoryBrowser(new DirectoryBrowserOptions
//{
//    FileProvider = new PhysicalFileProvider(@"E:\"),
//    RequestPath = "/server"
//});

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
