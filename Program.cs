using Microsoft.EntityFrameworkCore;
using DbAPI.Models;
using Microsoft.AspNetCore.Authentication;
using DbAPI.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var CnnctString=builder.Configuration.GetConnectionString("UserDB");
builder.Services.AddDbContext<StudentContext>(opt =>
    opt.UseSqlServer(CnnctString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions,BasicAuthenticationHandler>("BasicAuthentication",null);

var _jwtsecurity = builder.Configuration.GetSection("jwtSettings");
builder.Services.Configure<jwtSettings>(_jwtsecurity);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
