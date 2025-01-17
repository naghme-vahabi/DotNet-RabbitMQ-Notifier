using MessageBroker.Domain.Interfaces;
using MessageBroker.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<IMessageConsumer<T>, MessageConsumer<T>();
//builder.Services.AddSingleton< IMessagePoducer<T>, MessagePoducer<T>();
builder.Services.AddSingleton<EmailService>();

var app = builder.Build();

//var consumer = app.Services.GetRequiredService<IMessageConsumer<T>>();
//Task.Run(() => consumer.StartConsumingAsync(CancellationToken.None));
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
