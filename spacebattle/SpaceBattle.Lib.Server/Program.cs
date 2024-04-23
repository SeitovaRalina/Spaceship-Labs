using Microsoft.AspNetCore;

var builder = WebHost.CreateDefaultBuilder(args)
    .UseKestrel(options =>
    {
        options.ListenAnyIP(8080);
        options.ListenAnyIP(8443, listenOptions =>
        {
            options.AllowSynchronousIO = true;
            listenOptions.UseHttps();
        });
    })
    .UseStartup<Startup>();

var app = builder.Build();
app.Run();
