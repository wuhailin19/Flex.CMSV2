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
using Flex.Application.Middlewares;
using Flex.Application.SetupExtensions.OrmInitExtension;
using Flex.Core.Helper;
using Flex.EFSql;
using Flex.SqlSugarFactory;
using Flex.SqlSugarFactory.UnitOfWorks;
using Flex.WebApi.Jwt;
using Microsoft.AspNetCore.Diagnostics;
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
// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add(typeof(GlobalExceptionFilter)));
builder.Services.AddControllersWithViews().AddMvcOptions(options =>
{
    // 设置默认的响应字符编码为 UTF-8
    options.OutputFormatters.OfType<StringOutputFormatter>().FirstOrDefault().SupportedEncodings.Add(Encoding.UTF8);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//注册swagger
builder.Services.AddSwagger(AssemblyName);
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
        Mappings = { [".less"] = "text/less", [".html"]="text/html" }
    };
});
LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");
builder.Host.UseNLog();
builder.Services.AddLogging();
var app = builder.Build();

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
//app.UseMiddleware<FileProviderMiddleware>(builder.Environment.WebRootPath);
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

app.UseEndpoints(options =>
{
    options.MapControllerRoute(
        name: "AreaRoute",
        pattern: "{area:exists}/{controller=Login}/{action=Index}/{id?}");

    options.MapControllers();
});

app.Run();

