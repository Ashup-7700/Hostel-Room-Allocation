using Kemar.HRM.API.AutoMapper;
using Kemar.HRM.Business.FeeStructureBusiness;
using Kemar.HRM.Business.PaymentBusiness;
using Kemar.HRM.Business.RoomAllocationBusiness;
using Kemar.HRM.Business.RoomBusiness;
using Kemar.HRM.Business.StudentBusiness;
using Kemar.HRM.Business.UserBusiness;
using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Interface;
using Kemar.HRM.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HostelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IStudent, StudentRepository>();
builder.Services.AddScoped<IStudentManager, StudentManager>();

builder.Services.AddScoped<IRoom, RoomRepository>();
builder.Services.AddScoped<IRoomManager, RoomManager>();

builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<IUserManager, UserManager>();

builder.Services.AddScoped<IRoomAllocation, RoomAllocationRepository>();
builder.Services.AddScoped<IRoomAllocationManager, RoomAllocationManager>();


builder.Services.AddScoped<IPayment, PaymentRepository>();
builder.Services.AddScoped<IPaymentManager, PaymentManager>();


builder.Services.AddScoped<IFeeStructure, FeeStructureRepository>();
builder.Services.AddScoped<IFeeStructureManager, FeeStructureManager>();




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
