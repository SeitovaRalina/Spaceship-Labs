using CoreWCF;
using CoreWCF.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using WebHttp;

internal sealed class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddServiceModelWebServices(o =>
        {
            o.Title = "SpaceBattle Service API";
            o.Version = "1.0.0";
            o.Description = "Endpoint for processing incoming orders";
        });

        services.AddSingleton(new SwaggerOptions());
    }

    public static void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<SwaggerMiddleware>();
        app.UseSwaggerUI();

        app.UseServiceModel(builder =>
        {
            builder.AddService<WebApi>();
            builder.AddServiceWebEndpoint<WebApi, IWebApi>(new WebHttpBinding
            {
                MaxReceivedMessageSize = 5242880,
                MaxBufferSize = 65536
            }, "api", behavior =>
            {
                behavior.HelpEnabled = true;
                behavior.AutomaticFormatSelectionEnabled = true;
            });
        });
    }
}
