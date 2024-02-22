using LabSid.Infra;
using LabSid.Infra.Interfaces;
using LabSid.Migrations;
using LabSid.Services;
using LabSid.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string is empty!");
var environment = builder.Configuration.GetValue<string>("MigrationEnvironment");


// Postgres
builder.Services.AddScoped<IPostgresqlContext>(d => new PostgresqlContext(connectionString));

//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


//Services
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "LabSid Api", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    s.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });


});

builder.Services.AddCors(options => options.AddPolicy("AllowAll",
    builder => builder.AllowAnyMethod()
                      .AllowAnyMethod()
                      .AllowAnyOrigin()));



builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    try
    {
        var runner = new Migrator(true, connectionString);

        await runner.Migrate();
    }
    catch (Exception ex)
    {
        throw;
    }

}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1/swagger.json", "LabSid | API - v1");
});

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
