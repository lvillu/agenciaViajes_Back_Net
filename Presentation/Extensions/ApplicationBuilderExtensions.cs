namespace Presentation.Extensions
{
  public static class ApplicationBuilderExtensions
  {
    public static void UseSwaggerDocumentation(this IApplicationBuilder app)
    {
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proyecto de BackEnd Agencia de Viajes (Portal Admin)");
      });
    }
  }
}
