using Presentation.Endpoints;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAplicationService();
builder.Services.AddInfrastucture(builder.Configuration);
builder.Services.AddJWTAuth(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();


//builder.Services.AddAuthentication();

// Configuración de Autorización
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseSwaggerDocumentation();


// Habilita CORS
//app.UseCors();

// Uso de Middleware de Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

#region Mapeo los endpoints

app.MapUsersEndpoints();
app.MapLoginEndPoints();

#endregion



app.Run();
