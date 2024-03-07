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
using Flex.Core.Helper;
using Flex.Dapper;
using Flex.EFSqlServer;
using Flex.EFSqlServer.Register;
using Flex.Web.Jwt;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using NLog.Web;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add(typeof(GlobalExceptionFilter)));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//ע��swagger
builder.Services.AddSwagger(AssemblyName);
//ע����ͼ������
builder.Services.AddControllersWithViews();
//ע��EFcoreSqlserver
builder.Services.AddUnitOfWorkService<SqlServerContext>(item => item.UseSqlServer("DataConfig:Sqlserver:ConnectionString".Config(string.Empty)));

builder.Services.AddSingleton<MyDBContext>();

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
        builder.RegisterAutoFacExtension();
        //ע��ҵ��㣬ͬʱ��ҵ���ķ�����������
        builder.RegisterAssemblyTypes(Assembly.Load("Flex.Application"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()//����Autofac.Extras.DynamicProxy;// ע�ᱻ���ص��ಢ����������
            .InterceptedBy(typeof(LogInterceptor));//����ֻ��ͬ���ģ���Ϊ�첽������������������ͬ�������� 
    });

builder.Services.AddJwtService(builder.Configuration);
//ע��webcore������վ��Ҫ���ã�
builder.Services.AddWebCoreService(builder.Configuration);
//ע�Ỻ��
builder.Services.AddMemoryCacheSetup();
builder.Services.AddLogging();

// ����Զ���� MIME ����
builder.Services.Configure<StaticFileOptions>(options =>
{
    options.ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        // ��� Less �ļ��� MIME ����ӳ��
        Mappings = { [".less"] = "text/less" }
    };
});

builder.Host.UseNLog();

var app = builder.Build();


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
    // ��ʼ���ӿ�����
    var myService = app.Services.GetRequiredService<IRoleUrlServices>();
    await myService.CreateUrlList();
}

app.UseStaticFiles();

//app.UseMiddleware<FileProviderMiddleware>(builder.Environment.WebRootPath);

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
