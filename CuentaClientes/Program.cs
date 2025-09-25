using CuentaClientes.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios usando la clase de configuraci�n
ServiceConfiguration.ConfigureServices(builder);

var app = builder.Build();

// Configurar pipeline usando la clase de configuraci�n
PipelineConfiguration.ConfigurePipeline(app);

app.Run();