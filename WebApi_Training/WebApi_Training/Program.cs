using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebApi_Training;
using WebApi_Training.Database;
using WebApi_Training.Dto;
using WebApi_Training.Models;
using WebApi_Training.Repository;
using WebApi_Training.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", async (ICouponRepository _couponRepository) => { 
    return Results.Ok(await _couponRepository.GetAllAsync());
}) ;

app.MapGet("/api/coupon/{id:int}", async (ICouponRepository _couponRepository, int id) =>
{
    return Results.Ok(await _couponRepository.GetById(id)); 
});   

app.MapPost("/api/coupon/create", async ([FromBody] CreateCouponDto createCouponDto, ICouponRepository _couponRepository) =>
{
    var queryRecord = await _couponRepository.Create(createCouponDto);
    return Results.Ok(queryRecord);
});

app.MapPut("/api/coupon/update/{id}", async (ICouponRepository _couponRepository, UpdateCouponDto updateCouponDto, int id) =>
{
    var record = await _couponRepository.Update(updateCouponDto, id);
    return Results.Ok(record);
});

app.MapDelete("/api/coupon/delete/{id}", (ICouponRepository _couponRepository, int id) =>
{
     _couponRepository.DeleteById(id);
});

app.MapPost("api/login", async (IAuthenticationRepository _authenticationRepository,[FromBody] LoginRequestDto loginRequest) =>
{
    var loginResponse = await _authenticationRepository.Login(loginRequest);
    if (loginResponse == null)
        return null;
    return loginResponse;
});

app.MapPost("api/register", async (IAuthenticationRepository _authenticationRepository, [FromBody] RegisterRequestDto registerRequest) =>
{
    bool isUnique = _authenticationRepository.IsUnquine(registerRequest.UserName);
    if (!isUnique)
        return null;

    var registerResponse = await _authenticationRepository.Register(registerRequest);
    if (registerResponse == null || string.IsNullOrEmpty(registerResponse.UserName))
        return null;

    return registerResponse;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
