using Kemar.HRM.API.AutoMapper;
using Kemar.HRM.Business.RoomAllocationBusiness;
using Kemar.HRM.Business.RoomBusiness;
using Kemar.HRM.Business.StudentBusiness;
using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Interface;
using Kemar.HRM.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HostelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Student
builder.Services.AddScoped<IStudent, StudentRepository>();
builder.Services.AddScoped<IStudentManager, StudentManager>();

// Room
builder.Services.AddScoped<IRoom, RoomRepository>();
builder.Services.AddScoped<IRoomManager, RoomManager>();

// Room Allocation
builder.Services.AddScoped<IRoomAllocation, RoomAllocationRepository>();
builder.Services.AddScoped<IRoomAllocationManager, RoomAllocationManager>(); // ← REQUIRED

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
