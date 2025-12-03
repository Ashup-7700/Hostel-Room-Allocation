using Kemar.HRM.API.AutoMapper;

using Kemar.HRM.Business.RoomBusiness;
using Kemar.HRM.Business.StudentBusiness;
using Kemar.HRM.Business.UserBusiness;
using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Interface;
using Kemar.HRM.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -------------------- DB CONTEXT --------------------
builder.Services.AddDbContext<HostelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -------------------- AUTOMAPPER --------------------
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// -------------------- STUDENT --------------------
builder.Services.AddScoped<IStudent, StudentRepository>();
builder.Services.AddScoped<IStudentManager, StudentManager>();

// -------------------- ROOM --------------------
builder.Services.AddScoped<IRoom, RoomRepository>();
builder.Services.AddScoped<IRoomManager, RoomManager>();

//// -------------------- ROOM ALLOCATION --------------------
//builder.Services.AddScoped<IRoomAllocation, RoomAllocationRepository>();
//builder.Services.AddScoped<IRoomAllocationManager, RoomAllocationManager>();

// -------------------- USER (NEWLY ADDED) --------------------
builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<IUserManager, UserManager>();

// -------------------- CONTROLLERS + SWAGGER --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// -------------------- MIDDLEWARE --------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
