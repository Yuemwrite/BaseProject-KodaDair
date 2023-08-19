

using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("ocelot.json")
    .Build();

builder.Services.AddOcelot(configuration);
builder.Services.AddSwaggerForOcelot(configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});
app.UseOcelot().Wait();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); // Add it here
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();


