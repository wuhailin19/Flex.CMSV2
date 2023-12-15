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
//ע��swagger
builder.Services.AddSwagger(AssemblyName);
//ע����ͼ������
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    // ���������� JsonSerializerOptions
    options.JsonSerializerOptions.Converters.Add(new BooleanConverter());

    // ����������ã�����еĻ�
}); ;
//ע��EFcoreSqlserver
builder.Services.AddUnitOfWorkService<SqlServerContext>(item => item.UseSqlServer("DataConfig:Sqlserver:ConnectionString".Config(string.Empty)));

//ע��Automapper
builder.Services.AddAutomapperService();
//��������
builder.Services.AddCorsPolicy(builder.Configuration);
//ע��autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).
    ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterAutoFacExtension();
        //ע��ҵ��㣬ͬʱ��ҵ���ķ�����������
        builder.RegisterAssemblyTypes(Assembly.Load("Flex.Application"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    });

builder.Services.AddJwtService(builder.Configuration);
//ע��webcore������վ��Ҫ���ã�
builder.Services.AddWebCoreService(builder.Configuration);
//ע�Ỻ��
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
