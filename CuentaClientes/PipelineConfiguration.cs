namespace CuentaClientes.Configurations
{
    public static class PipelineConfiguration
    {
        public static void ConfigurePipeline(WebApplication app)
        {
            // Configurar Swagger UI en desarrollo
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CuentaClientes API V1");
                    options.RoutePrefix = string.Empty;
                });
            }

            // Redirigir la ruta base a Swagger
            app.MapGet("/", () => Results.Redirect("/swagger"));

            // Pipeline HTTP standard
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}