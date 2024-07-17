using System.Text;
using System.Text.Json.Serialization;
using api.Data;
using api.Services;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

DotEnv.Load(new DotEnvOptions(envFilePaths: [".env", ".env.local"]));

var builder = WebApplication.CreateBuilder(args);

{
    var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
    if (!secret.IsNullOrEmpty()) builder.Configuration.GetSection("Authentication:Bearer:Key").Value = secret;

    var db_host = Environment.GetEnvironmentVariable("DB_HOST");
    var db_port = Environment.GetEnvironmentVariable("DB_PORT");
    var db_database = Environment.GetEnvironmentVariable("DB_DATABASE");
    var db_username = Environment.GetEnvironmentVariable("DB_USERNAME");
    var db_password = Environment.GetEnvironmentVariable("DB_PASSWORD");

    if (db_port.IsNullOrEmpty()) db_port = "5432";

    if (!new[] { db_host, db_database, db_username }.Any(string.IsNullOrEmpty))
        builder.Configuration.GetSection("ConnectionStrings:Default").Value ??=
            $"Host={db_host};Port={db_port};Database={db_database};Username={db_username};Password={db_password}";
}

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var bearer = builder.Configuration.GetSection("Authentication:Bearer");
        var key = bearer["Key"] ?? throw new InvalidOperationException("JWT key is not set");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = bearer["Issuer"],
            ValidAudience = bearer["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<ContactCategoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
