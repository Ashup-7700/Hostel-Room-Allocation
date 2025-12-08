using Kemar.HRM.API.AutoMapper;
using Kemar.HRM.Business.FeeStructureBusiness;
using Kemar.HRM.Business.PaymentBusiness;
using Kemar.HRM.Business.RoomAllocationBusiness;
using Kemar.HRM.Business.RoomBusiness;
using Kemar.HRM.Business.StudentBusiness;
using Kemar.HRM.Business.UserBusiness;
using Kemar.HRM.Business.UserTokenBusiness;
using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Interface;
using Kemar.HRM.Repository.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<HostelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IStudent, StudentRepository>();
builder.Services.AddScoped<IStudentManager, StudentManager>();

builder.Services.AddScoped<IRoom, RoomRepository>();
builder.Services.AddScoped<IRoomManager, RoomManager>();

builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<IUserManager, UserManager>();

builder.Services.AddScoped<IUserToken, UserTokenRepository>();
builder.Services.AddScoped<IUserTokenManager, UserTokenManager>();

builder.Services.AddScoped<IRoomAllocation, RoomAllocationRepository>();
builder.Services.AddScoped<IRoomAllocationManager, RoomAllocationManager>();

builder.Services.AddScoped<IPayment, PaymentRepository>();
builder.Services.AddScoped<IPaymentManager, PaymentManager>();

builder.Services.AddScoped<IFeeStructure, FeeStructureRepository>();
builder.Services.AddScoped<IFeeStructureManager, FeeStructureManager>();

builder.Services.AddControllers();

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kemar.HRM API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT token.",
    };

    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

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
