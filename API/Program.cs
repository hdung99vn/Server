using API.Builder;
using API.ServiceRegister;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.AddBuilder(app.Environment.IsDevelopment());
//app.MapControllers();
app.MapControllers();//.RequireAuthorization("SampleAPI");
await app.RunAsync();