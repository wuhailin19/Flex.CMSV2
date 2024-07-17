using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Flex.Application.Aop;
using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.ISignalRBus;
using Flex.Application.Contracts.ISignalRBus.Queue;
using Flex.Application.Exceptions;
using Flex.Application.Extensions.Register.AutoMapper;
using Flex.Application.Extensions.Register.MemoryCacheExtension;
using Flex.Application.Extensions.Register.WebCoreExtensions;
using Flex.Application.Extensions.Swagger;
using Flex.Application.Middlewares;
using Flex.Application.SetupExtensions.OrmInitExtension;
using Flex.Application.SignalRBus.Hubs;
using Flex.Application.SignalRBus.Queue;
using Flex.Application.SignalRBus.Services;
using Flex.Core.Helper;
using Flex.SingleWeb.Components;
using Flex.SqlSugarFactory;
using Flex.SqlSugarFactory.UnitOfWorks;
using Flex.WebApi.Jwt;
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
using Microsoft.Extensions.Hosting;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var builder = WebApplication.CreateBuilder(args);
var AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add(typeof(GlobalExceptionFilter)));

builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddMvcOptions(options =>
{
    // ����Ĭ�ϵ���Ӧ�ַ�����Ϊ UTF-8
    options.OutputFormatters.OfType<StringOutputFormatter>().FirstOrDefault().SupportedEncodings.Add(Encoding.UTF8);
});
// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
//ע��swagger
.AddSwagger(AssemblyName)
//ע��EFcoreSqlserver
.RegisterDbConnectionString()
//ע��Automapper
.AddAutomapperService()
//��������
.AddCorsPolicy()
//ע��jwt
.AddJwtService()
//ע��ѩ���㷨
.AddWebCoreService()
//ע�Ỻ��
.AddMemoryCacheSetup();

builder.Services.AddSignalR();
builder.Services.AddHostedService<ExportBackgroundService>();

string webpath = builder.Environment.WebRootPath;
//builder.Services.HtmlTemplateDictInit();
//ע��autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).
    ConfigureContainer<ContainerBuilder>(builder =>
    {
        //ע����������ͬ���첽��Ҫ
        builder.RegisterType<LogInterceptor>().AsSelf();
        builder.RegisterType<LogInterceptorAsync>().AsSelf();
        builder.RegisterType<PhysicalFileProvider>().As<IFileProvider>().WithParameter("root", webpath).SingleInstance();

        builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency(); //ע��Sqlsugar�ִ�
        //ע��ҵ��㣬ͬʱ��ҵ���ķ�����������
        builder.RegisterAssemblyTypes(Assembly.Load("Flex.Application"))
             .Where(t => !t.Name.EndsWith("SqlTableServices")&& !t.Name.EndsWith("Queue")) // �ų��� "SqlTableServices��Queue" ��β������
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()//����Autofac.Extras.DynamicProxy;// ע�ᱻ���ص��ಢ����������
            .InterceptedBy(typeof(LogInterceptor));//����ֻ��ͬ���ģ���Ϊ�첽������������������ͬ�������� 

        builder.RegisterType<ConcurrentQueue<ExportRequest>>().As<IConcurrentQueue<ExportRequest>>().SingleInstance();

        builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
    });
// ����Զ���� MIME ����
builder.Services.Configure<StaticFileOptions>(options =>
{
    options.ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        // ��� Less �ļ��� MIME ����ӳ��
        Mappings = { [".less"] = "text/less", [".html"] = "text/html" }
    };
});


LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");
builder.Host.UseNLog();
builder.Services.AddLogging();
// ���� Kestrel ������ѡ��
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 2147483648; // ��������Ϊ����Ҫ�����ֵ
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 2147483648; // ��������Ϊ����Ҫ�����ֵ
});

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
    // ��ʼ���ӿ�����
    var myService = app.Services.GetRequiredService<IRoleUrlServices>();
    await myService.CreateUrlList();
}
//// ��ʼ���Զ��������ݿ�
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<EfCoreDBContext>();

//    // ȷ�����ݿ��Ѵ���
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
            JsonHelper.ToJson(new Message<string> { 
                code = ErrorCodes.NoOperationPermission.ToInt(), 
                msg = ErrorCodes.NoOperationPermission.GetEnumDescription() 
            }));
    }
    return Task.CompletedTask;
});

//������Ҫ�ŵ���ǰ�棬�Ծ�̬�ļ�����Ч
app.UseCors(WebCoreSetupExtension.MyAllowSpecificOrigins);
//��д�ŵ�StaticFilesǰ��
app.UseRewritePathMiddleware();
var options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("index.html");
options.DefaultFileNames.Add("*.html");
app.UseDefaultFiles(options);
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    // ���þ�̬�ļ��� Content-Type
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

// ����Զ����м���������·���ض��򵽵�¼ҳ
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/system")
    {
        // �ض��򵽵�¼ҳ
        context.Response.Redirect("/System/Login/Index");
        return;
    }
    await next();
});
app.UseAntiforgery();

app.MapAreaControllerRoute(
        name: "AreaRoute",
        areaName: "System",
        pattern: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// ע�� SignalR Hub
app.MapHub<ExportHub>("/exportHub");

app.Run();

