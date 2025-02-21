using AgendaWeb.Infra.Data.Interfaces;
using AgendaWeb.Infra.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configurando o projeto para MVC
builder.Services.AddControllersWithViews();

//habilitando o uso de sessões no projeto
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//capturar a connectionstring mapeada no 'appsettings.json'
var connectionString = builder.Configuration.GetConnectionString("AgendaWeb");

//enviar a connectionstring para a classe EventoRepository
builder.Services.AddTransient<IEventoRepository>(map => new EventoRepository(connectionString));
builder.Services.AddTransient<IUsuarioRepository>(map => new UsuarioRepository(connectionString));

var app = builder.Build();

//habilitando o uso de sessões no projeto
app.UseSession();

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
    pattern: "{controller=Account}/{action=login}"
    );

app.Run();
