using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Flex.Application.Aop;
using Flex.Application.Contracts.Exceptions;
using Flex.Application.Exceptions;
using Flex.Application.Extensions.AutoFacExtension;
using Flex.Application.Extensions.Register.AutoMapper;
using Flex.Application.Extensions.Register.MemoryCacheExtension;
using Flex.Application.Extensions.Register.WebCoreExtensions;
using Flex.Application.Extensions.Swagger;
using Flex.Application.SetupExtensions.OrmInitExtension;
using Flex.Core.Helper;
using Flex.SqlSugarFactory;
using Flex.Web.Jwt;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using NLog;
using NLog.Web;
using System.Reflection;
using NLog.Config;
using Autofac.Core;
using Flex.SqlSugarFactory.UnitOfWorks;
using Microsoft.Extensions.Hosting;
using Flex.EFSql;
using Microsoft.AspNetCore.Builder;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var builder = WebApplication.CreateBuilder(args);
var AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add(typeof(GlobalExceptionFilter)));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//注册swagger
builder.Services.AddSwagger(AssemblyName);
//注册视图控制器
//builder.Services.AddControllersWithViews();
//注册EFcoreSqlserver
builder.Services.RegisterDbConnectionString();
//注册Automapper
builder.Services.AddAutomapperService();
//跨域配置
builder.Services.AddCorsPolicy(builder.Configuration);

string webpath = builder.Environment.WebRootPath;
//builder.Services.HtmlTemplateDictInit();
//注册autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).
    ConfigureContainer<ContainerBuilder>(builder =>
    {
        //注册拦截器，同步异步都要
        builder.RegisterType<LogInterceptor>().AsSelf();
        builder.RegisterType<LogInterceptorAsync>().AsSelf();
        builder.RegisterType<PhysicalFileProvider>().As<IFileProvider>().WithParameter("root", webpath).SingleInstance();

        builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency(); //注册Sqlsugar仓储
        builder.RegisterAutoFacExtension();
        //注册业务层，同时对业务层的方法进行拦截
        builder.RegisterAssemblyTypes(Assembly.Load("Flex.Application"))
             .Where(t => !t.Name.EndsWith("SqlTableServices")) // 排除以 "SqlTableServices" 结尾的类型
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;// 注册被拦截的类并启用类拦截
            .InterceptedBy(typeof(LogInterceptor));//这里只有同步的，因为异步方法拦截器还是先走同步拦截器 

        builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
    });

builder.Services.AddJwtService(builder.Configuration);
//注册webcore服务（网站主要配置）
builder.Services.AddWebCoreService(builder.Configuration);
//注册缓存
builder.Services.AddMemoryCacheSetup();

// 添加自定义的 MIME 类型
builder.Services.Configure<StaticFileOptions>(options =>
{
    options.ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        // 添加 Less 文件的 MIME 类型映射
        Mappings = { [".less"] = "text/less" }
    };
});
LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");
builder.Host.UseNLog();
builder.Services.AddLogging();

var app = builder.Build();

// 初始化自动创建数据库
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EfCoreDBContext>();

    // 确保数据库已创建
    context.Database.EnsureCreated();
}

app.UseStatusCodePages((StatusCodeContext statusCodeContext) =>
{
    var context = statusCodeContext.HttpContext;
    if (context.Response.StatusCode == 401 || context.Response.StatusCode == 403)
    {
        context.Response.StatusCode = ErrorCodes.NoOperationPermission.ToInt();
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(JsonHelper.ToJson(new Message<string> { code = ErrorCodes.NoOperationPermission.ToInt(), msg = ErrorCodes.NoOperationPermission.GetEnumDescription() }));
    }

    return Task.CompletedTask;
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AssemblyName} v1");
        c.RoutePrefix = "swagger";
    });
    // 初始化接口数据
    var myService = app.Services.GetRequiredService<IRoleUrlServices>();
    await myService.CreateUrlList();
}

app.UseStaticFiles();

//app.UseMiddleware<FileProviderMiddleware>(builder.Environment.WebRootPath);

app.UseRouting();
app.UseAuthentication();

app.UseCors(WebCoreSetupExtension.MyAllowSpecificOrigins);
app.UseAuthorization();
app.UseEndpoints(options => {
    options.MapControllerRoute(
        name: "default",
        pattern: "/api/{controller=Home}/{action=Index}/{id?}");
});
app.Run();
