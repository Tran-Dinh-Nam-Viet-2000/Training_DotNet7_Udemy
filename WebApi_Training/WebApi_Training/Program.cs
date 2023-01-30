using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_Training;
using WebApi_Training.Database;
using WebApi_Training.Dto;
using WebApi_Training.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", (ApplicationDbContext dbContext) => { 
    return Results.Ok(dbContext.coupons.ToList());
}) ;

app.MapGet("/api/coupon/{id:int}", (ApplicationDbContext dbContext,int id) =>
{
    return Results.Ok(dbContext.coupons.FirstOrDefault(n => n.Id == id)); 
});   

app.MapPost("/api/coupon/create", ([FromBody] CreateCouponDto createCouponDto, ApplicationDbContext dbContext) =>
{
    var queryRecord = dbContext.coupons.FirstOrDefault(n => n.Name == createCouponDto.Name);
    if (queryRecord != null)
    {
        return Results.BadRequest("Record already exits");
    }
    Coupon coupon = new()
    {
        Name = createCouponDto.Name,
        IsActive = createCouponDto.IsActive,
        Percent = createCouponDto.Percent,
        Created = createCouponDto.Created
    };
    dbContext.coupons.Add(coupon);
    dbContext.SaveChanges();
    return Results.Ok(coupon);
});

app.MapPut("/api/coupon/update/{id}", (ApplicationDbContext dbContext, UpdateCouponDto updateCoupon, int id) =>
{
    var record = dbContext.coupons.FirstOrDefault(n => n.Id == id);
    if (record == null)
    {
        return Results.BadRequest("Record doesn't exits");
    }
    else
    {
        record.Name = updateCoupon.Name;
        record.Percent = updateCoupon.Percent;
        record.IsActive = updateCoupon.IsActive;
        record.LastUpdated = updateCoupon.LastUpdated;
        dbContext.SaveChanges();
    }
    return Results.Ok(record);
});

app.MapDelete("/api/coupon/delete/{id}", (ApplicationDbContext dbContext, int id) =>
{
    var queryId = dbContext.coupons.FirstOrDefault(n => n.Id == id);
    if (queryId == null)
    {
        return Results.BadRequest("Id does not exist");
    }
    else
    {
        dbContext.coupons.Remove(queryId);
        dbContext.SaveChanges();
        return Results.Ok("Deleted record");
    }    
});
 
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
