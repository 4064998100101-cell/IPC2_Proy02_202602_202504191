using Proyecto2;

var builder = WebApplication.CreateBuilder(args);

// Habilitar Razor Pages
builder.Services.AddRazorPages();

// Registrar tus clases principales como servicios de memoria compartida (Singleton)
builder.Services.AddSingleton<Catalogo>();
builder.Services.AddSingleton<Xml>(); // O Xml si tu clase se llama lectorXml, ajusta según el nombre de tu archivo

var app = builder.Build();

// Configuración del entorno HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Mapear las páginas de la carpeta Pages
app.MapRazorPages();

// Hacer que la página principal (Index o raíz '/') redirija o cargue directamente tu Catalogo
app.MapGet("/", async context =>
{
    context.Response.Redirect("/Catalogo");
    await Task.CompletedTask;
});

app.Run();