using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

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
