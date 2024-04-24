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
    // ����Ĭ�ϵ���Ӧ�ַ�����Ϊ UTF-8
    options.OutputFormatters.OfType<StringOutputFormatter>().FirstOrDefault().SupportedEncodings.Add(Encoding.UTF8);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//ע��swagger
builder.Services.AddSwagger(AssemblyName);
//ע��EFcoreSqlserver
builder.Services.RegisterDbConnectionString();
//ע��Automapper
builder.Services.AddAutomapperService();
//��������
builder.Services.AddCorsPolicy(builder.Configuration);

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
        builder.RegisterAutoFacExtension();
        //ע��ҵ��㣬ͬʱ��ҵ���ķ�����������
        builder.RegisterAssemblyTypes(Assembly.Load("Flex.Application"))
             .Where(t => !t.Name.EndsWith("SqlTableServices")) // �ų��� "SqlTableServices" ��β������
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()//����Autofac.Extras.DynamicProxy;// ע�ᱻ���ص��ಢ����������
            .InterceptedBy(typeof(LogInterceptor));//����ֻ��ͬ���ģ���Ϊ�첽������������������ͬ�������� 

        builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
    });

builder.Services.AddJwtService(builder.Configuration);
//ע��webcore������վ��Ҫ���ã�
builder.Services.AddWebCoreService(builder.Configuration);
//ע�Ỻ��
builder.Services.AddMemoryCacheSetup();

// ����Զ���� MIME ����
builder.Services.Configure<StaticFileOptions>(options =>
{
    options.ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        // ��� Less �ļ��� MIME ����ӳ��
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
    // ��ʼ���ӿ�����
    var myService = app.Services.GetRequiredService<IRoleUrlServices>();
    await myService.CreateUrlList();


}

// ��ʼ���Զ��������ݿ�
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EfCoreDBContext>();

    // ȷ�����ݿ��Ѵ���
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
//app.UseMiddleware<FileProviderMiddleware>(builder.Environment.WebRootPath);
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

app.UseEndpoints(options =>
{
    options.MapControllerRoute(
        name: "AreaRoute",
        pattern: "{area:exists}/{controller=Login}/{action=Index}/{id?}");

    options.MapControllers();
});

app.Run();

