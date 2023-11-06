using grupoBalPruebaAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//Jwt configuración
var jwtIssuer = builder.Configuration.GetSection("Jwt:ValidIssuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Secret").Get<string>();
var jwtAudience = builder.Configuration.GetSection("Jwt:ValidAudience").Get<string>();
var corsServer = builder.Configuration.GetSection("CORS:frontDev").Get<string>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .WithOrigins(corsServer) // Reemplaza con el origen de tu aplicación Angular
        .AllowAnyHeader()
        .AllowAnyMethod());
});
// Configurar CORS FIN


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtAudience,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });

//Jwt configuración fin


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<GbPruebaContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
