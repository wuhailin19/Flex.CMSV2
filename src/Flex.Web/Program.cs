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
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Reflection;
using Dapper.EntityFramework;

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

builder.Services.HtmlTemplateDictInit();
//ע��autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).
    ConfigureContainer<ContainerBuilder>(builder =>
    {
        //ע����������ͬ���첽��Ҫ
        builder.RegisterType<LogInterceptor>().AsSelf();
        builder.RegisterType<LogInterceptorAsync>().AsSelf();
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
