using Certificates_Platform;
using ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

// MVC + Views
builder.Services.AddControllersWithViews();

// Dependency Injection
new RegisterOptions().Compose(builder, typeof(Program).Assembly);
new RegisterServices(builder, typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // for wwwroot
app.UseRouting();
app.UseAuthorization();

// ROUTE TO CONTROLLERS + VIEWS
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
