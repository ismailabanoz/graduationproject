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
        /*services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("SqlCon"));

        });*/
        //services.AddSingleton(typeof(ICustomerService), typeof(CustomerManager));
        //services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<ICustomerService, CustomerManager>();
        services.AddSingleton<PicturesWatermarkProcess>();
        //services.AddScoped(typeof(ConnectionFactory));
        //services.AddScoped(typeof(DirectExcenge));
        services.AddSingleton<RabbitMQClientService>(); 
        //services.AddSingleton<RabbitMQPublisher>();
        services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(Configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });
        //services.AddSingleton<IAddWatermarkService, AddWatermarkService>();
        //services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddHostedService<Worker>();
        services.AddHostedService<PicturesWatermarkProcess>();
        services.AddHostedService<CreatePictureProcess>();
    })
    .Build();

await host.RunAsync();
