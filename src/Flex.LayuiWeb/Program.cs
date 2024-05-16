using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddMvcOptions(options =>
{
    // ����Ĭ�ϵ���Ӧ�ַ�����Ϊ UTF-8
    options.OutputFormatters.OfType<StringOutputFormatter>().FirstOrDefault().SupportedEncodings.Add(Encoding.UTF8);
});

var app = builder.Build();

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
    if (context.Request.Path == "/")
    {
        // �ض��򵽵�¼ҳ
        context.Response.Redirect("/System/Login/Index");
        return;
    }

    await next();
});
#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "System/{controller=Login}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "AreaRoute",
        pattern: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
});
#pragma warning restore ASP0014 // Suggest using top level route registrations
app.Run();
