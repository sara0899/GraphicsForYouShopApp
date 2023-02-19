using GraphicsForYouShopApi.Models;
using GraphicsForYouShopApi.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder
    .WithOrigins("http://192.168.0.141:8001", "http://192.168.0.141:5555")
    .WithMethods("GET", "POST")
    .AllowCredentials()
    .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GraphicsDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("GraphicsForYouShopDatabase")
    ));



builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseWebSockets();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseCors();

//app.MapControllers();

//app.MapHub<CommunicatorHub>("/chathub", options =>
//{
//    options.Transports =
//        HttpTransportType.WebSockets |
//        HttpTransportType.LongPolling;
//}
//);

app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
    endpoints.MapHub<CommunicatorHub>("/CommunicatorHub"); //tu dopisaæ ca³¹ œcie¿kê localhost i ip/??
});


app.Run();
