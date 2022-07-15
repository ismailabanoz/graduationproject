using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RegistrationDirectory.DataAccess.Absract;
using RegistrationDirectory.DataAccess.Concrete;
using RegistrationDirectory.Service.Absract;
using RegistrationDirectory.Service.Concrete;
using RegistrationDirectory.Watermark;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {

        IConfiguration Configuration = context.Configuration;


        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("SqlCon"));

        });
        services.AddSingleton<RabbitMQClientService>(); 
        services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(Configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });
        services.AddHostedService<Worker>();
        services.AddHostedService<PicturesWatermarkProcess>();
        services.AddHostedService<CreatePictureProcess>();
    })
    .Build();

await host.RunAsync();
