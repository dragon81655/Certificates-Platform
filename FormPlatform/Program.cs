using ServiceRegistration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

new ServiceRegistration.RegisterServices(builder, Assembly.GetExecutingAssembly());
new RegisterOptions().Compose(builder, Assembly.GetExecutingAssembly());

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
