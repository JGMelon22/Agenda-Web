using AgendaWeb.Infra.Data.Interfaces;
using AgendaWeb.Infra.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configurando o projeto para MVC
builder.Services.AddControllersWithViews();

//capturar a connectionstring mapeada no 'appsettings.json'
var connectionString = builder.Configuration.GetConnectionString("AgendaWeb");

//enviar a connectionstring para a classe EventoRepository
builder.Services.AddTransient<IEventoRepository>
    (map => new EventoRepository(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Definindo a página inicial do projeto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}"
    );

app.Run();
