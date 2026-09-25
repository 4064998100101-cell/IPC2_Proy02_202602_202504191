using Proyecto2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();


builder.Services.AddSingleton<Catalogo>();
builder.Services.AddSingleton<Xml>(); 

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapRazorPages();


app.MapGet("/", async context =>
{
    context.Response.Redirect("/Catalogo");
    await Task.CompletedTask;
});

app.Run();