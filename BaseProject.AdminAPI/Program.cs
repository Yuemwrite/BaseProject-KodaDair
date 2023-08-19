using System.Reflection;
using BaseProject.Infrastructure;
using BaseProject.Infrastructure.Context;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((context, config) =>
{
    var env = context.HostingEnvironment;
    config
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();
});

builder.Services.AddControllers();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // options.UseMySql(builder.Configuration.GetConnectionString("SqlServer"),
    //     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql")),
    //     sqlOptions => { sqlOptions.MigrationsAssembly("Migratiors.Local"); });

    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"),
        sqlOptions => { sqlOptions.MigrationsAssembly("Migratiors.Local"); });
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);


});
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Seq("http://localhost:5341")
    .MinimumLevel.Verbose());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


await app.Services.InitializeDatabasesAsync();
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseInfrastructure();
app.UseAuthorization();
app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();