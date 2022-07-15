using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using RabbitMQ.Client;
using RegistrationDirectory.DataAccess.Absract;
using RegistrationDirectory.DataAccess.Concrete;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;
using RegistrationDirectory.Service.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<IReportService, ReportManager>();
builder.Services.AddScoped<ICommercialActivityService, CommercialActivityManager>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(AppDbContext));
builder.Services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")),DispatchConsumersAsync=true});
builder.Services.AddSingleton<RabbitMQClientService>();
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddScoped<ReportManager>();


builder.Services.AddHostedService<QuartzHostedService>();
builder.Services.AddSingleton<IJobFactory,SingletonJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory,StdSchedulerFactory>();
builder.Services.AddSingleton<JobReminders>();
builder.Services.AddSingleton<SecondJobReminder>();
builder.Services.AddSingleton(new MyJobWeekly(type:typeof(JobReminders),expression: "0 0 0 ? * SUN *"));//every 5 second 0 / 5 0 / 1 * 1 / 1 * ? * - every week 0 0 0 ? * SUN *
builder.Services.AddSingleton(new MyJobMonthly(type:typeof(SecondJobReminder),expression: "0 0 0 1 * ? *"));//every 15 second 0 / 15 0 / 1 * 1 / 1 * ? * - every month 0 0 0 1 * ? *



builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("SqlCon"));
});



builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
