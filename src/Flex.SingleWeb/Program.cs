using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Flex.Application.Aop;
using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.ISignalRBus.Queue;
using Flex.Application.Exceptions;
using Flex.Application.Extensions.Register.AutoMapper;
using Flex.Application.Extensions.Register.MemoryCacheExtension;
using Flex.Application.Extensions.Register.WebCoreExtensions;
using Flex.Application.Extensions.Swagger;
using Flex.Application.Middlewares;
using Flex.Application.SetupExtensions.OrmInitExtension;
using Flex.Application.SignalRBus.Factory;
using Flex.Application.SignalRBus.Hubs;
using Flex.Application.SignalRBus.Queue;
using Flex.Application.SignalRBus.Services;
using Flex.Core.Config;
using Flex.Core.Helper;
using Flex.Domain.Dtos.SignalRBus.Model.Request;
using Flex.EFSql;
using Flex.SingleWeb.Components;
using Flex.SqlSugarFactory;
using Flex.SqlSugarFactory.UnitOfWorks;
using Flex.WebApi.Jwt;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using NLog;
using NLog.Config;
using NLog.Web;
using System.Reflection;
using System.Text;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var builder = WebApplication.CreateBuilder(args);
var AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
var projectpath = AppDomain.CurrentDomain.BaseDirectory;
Directory.SetCurrentDirectory(projectpath);

// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add(typeof(GlobalExceptionFilter)));

builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddMvcOptions(options =>
{
    // 设置默认的响应字符编码为 UTF-8
    options.OutputFormatters.OfType<StringOutputFormatter>().FirstOrDefault().SupportedEncodings.Add(Encoding.UTF8);
});
// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
//注册swagger
.AddSwagger(AssemblyName)
//注册EFcoreSqlserver
.RegisterDbConnectionString()
//注册Automapper
.AddAutomapperService()
//跨域配置
.AddCorsPolicy()
//注册jwt
.AddJwtService()
//注册雪花算法
.AddWebCoreService()
//注册缓存
.AddMemoryCacheSetup();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict; // 设置全局的 SameSite 策略
    options.HttpOnly = HttpOnlyPolicy.Always;
    //options.CheckConsentNeeded = context => true; // 当需要用户同意时返回 true（取决于你的隐私政策）
});

builder.Services.AddSingleton<ConnectionStatus>();
builder.Services.AddSignalR();

builder.Services.AddHostedService<ExportBackgroundService>();
builder.Services.AddHostedService<ImportBackgroundService>();

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
        //注册业务层，同时对业务层的方法进行拦截
        builder.RegisterAssemblyTypes(Assembly.Load("Flex.Application"))
             .Where(t => !t.Name.EndsWith("SqlTableServices") && !t.Name.EndsWith("Queue")) // 排除以 "SqlTableServices和Queue" 结尾的类型
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;// 注册被拦截的类并启用类拦截
            .InterceptedBy(typeof(LogInterceptor));//这里只有同步的，因为异步方法拦截器还是先走同步拦截器 

        builder.RegisterType<ConcurrentQueue<ExportRequestModel>>().As<IConcurrentQueue<ExportRequestModel>>().SingleInstance();
        builder.RegisterType<ConcurrentQueue<ImportRequestModel>>().As<IConcurrentQueue<ImportRequestModel>>().SingleInstance();
        builder.RegisterType<ConcurrentQueue<RequestModel>>().As<IConcurrentQueue<RequestModel>>().SingleInstance();


        builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
    });

//初始化请求模型
builder.Services.InitRequesModelDict();

// 添加自定义的 MIME 类型
builder.Services.Configure<StaticFileOptions>(options =>
{
    options.ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        // 添加 Less 文件的 MIME 类型映射
        Mappings = { [".less"] = "text/less", [".html"] = "text/html" }
    };
});


LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");
builder.Host.UseNLog();
builder.Services.AddLogging();
// 配置 Kestrel 服务器选项
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 2147483648; // 2GB
    options.Limits.KeepAliveTimeout = new TimeSpan(0, 10, 0); //设置超时时间为10分钟
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 2147483648; // 2GB
});

var app = builder.Build();

app.UseCookiePolicy(); // 必须在其他中间件之前调用

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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
//// 初始化自动创建数据库
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<EfCoreDBContext>();

//    // 确保数据库已创建
//    context.Database.EnsureCreated();
//}
app.UseStatusCodePages((StatusCodeContext statusCodeContext) =>
{
    var context = statusCodeContext.HttpContext;
    if (context.Response.StatusCode == 401 || context.Response.StatusCode == 403)
    {
        context.Response.StatusCode = ErrorCodes.NoOperationPermission.ToInt();
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(
            JsonHelper.ToJson(new Message<string>
            {
                code = ErrorCodes.NoOperationPermission.ToInt(),
                msg = ErrorCodes.NoOperationPermission.GetEnumDescription()
            }));
    }
    return Task.CompletedTask;
});

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", ServerConfig.Content_Security_Policy);
    context.Response.Headers.Add("X-XSS-Protection", ServerConfig.X_XSS_Protection);
    context.Response.Headers.Add("Strict-Transport-Security", ServerConfig.Strict_Transport_Security);
    context.Response.Headers.Add("X-Content-Type-Options", ServerConfig.X_Content_Type_Options);
    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", ServerConfig.X_Permitted_Cross_Domain_Policies);

    context.Response.Headers.Add("X-Frame-Options", ServerConfig.X_Frame_Options);
    context.Response.Headers.Add("X-Download-Options", ServerConfig.X_Download_Options);
    context.Response.Headers.Add("Referrer-Policy", ServerConfig.Referrer_Policy);
    await next();
});

//跨域需要放到最前面，对静态文件才生效
app.UseCors(WebCoreSetupExtension.MyAllowSpecificOrigins);
//重写放到StaticFiles前面
app.UseRewritePathMiddleware();
var options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("index.html");
options.DefaultFileNames.Add("*.html");
app.UseDefaultFiles(options);
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    // 设置静态文件的 Content-Type
    ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        Mappings = {
            [".css"] = "text/css; charset=utf-8",
            [".js"] = "text/js; charset=utf-8",
        },
    }
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 添加自定义中间件来处理根路径重定向到登录页
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/system")
    {
        // 重定向到登录页
        context.Response.Redirect("/System/Login/Index");
        return;
    }
    await next();
});

// 在管道中添加防伪中间件
app.UseAntiforgery();

app.MapAreaControllerRoute(
        name: "AreaRoute",
        areaName: "System",
        pattern: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 注册 SignalR Hub
app.MapHub<UserHub>("/exportHub");

app.Run();

