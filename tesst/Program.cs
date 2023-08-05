using admin.Services.GuestTableService;
using admin.Services.UserAuthService;
using admin.Services.UserService;
using common.Helper.HashGenerator;
using common.Helper.TokenGenerator;
using common.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Redis;
using Redis.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//bwt akses env
ConfigurationManager configuration = builder.Configuration;

// connect ke mysql
builder.Services.AddDbContext<DemoDbContext>(_ =>
{
    _.UseMySql("server=localhost;port=3306;user=root;database=demo", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.25-mariadb"));
});

//redis
var redisOptions = configuration.GetSection("RedisServer").Get<RedisOption>();
if (redisOptions.Enabled)
    builder.Services.UseRedis(redisOptions.RedisCacheConfiguration);

// startup untuk DI injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAuthService, UserAuthService>();
builder.Services.AddScoped<IGuestTableService, GuestTableService>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IHashGenerator, HashGenerator>();

// tambah auth di swagger

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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
