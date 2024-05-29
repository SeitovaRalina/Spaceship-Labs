using Microsoft.AspNetCore;

var builder = WebHost.CreateDefaultBuilder(args)
    .UseKestrel(options =>
    {
        options.ListenAnyIP(8080);
    })
    .UseStartup<Startup>();

var app = builder.Build();
app.Run();
