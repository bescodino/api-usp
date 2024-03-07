using LabSid.Infra;
using LabSid.Infra.Interfaces;
using LabSid.Migrations;
using LabSid.Services;
using LabSid.Services.Auth;
using LabSid.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SQLitePCL;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string is empty!");
var environment = builder.Configuration.GetValue<string>("MigrationEnvironment");


//Sqlite
Batteries.Init();
builder.Services.AddScoped<ISqliteContext>(d => new SqliteContext(connectionString));

//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Mapper
builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddAuthorization(options =>
{

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("Users", policy =>
        policy.RequireRole("User"));
});

var key = Encoding.ASCII.GetBytes(Settings.Secret);

// Configure JWT authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

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

app.UseRouting(); // Ensure UseRouting is called before other middleware

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
