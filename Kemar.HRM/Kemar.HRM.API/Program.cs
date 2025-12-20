using Kemar.HRM.API.AutoMapper;
using Kemar.HRM.Business.FeeStructureBusiness;
using Kemar.HRM.Business.Interface;
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
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------
// Add Services
// ----------------------------------------

builder.Services.AddHttpContextAccessor();

// DbContext
builder.Services.AddDbContext<HostelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers + JSON
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Dependency Injection
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
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentManager, PaymentManager>();
builder.Services.AddScoped<IFeeStructure, FeeStructureRepository>();
builder.Services.AddScoped<IFeeStructureManager, FeeStructureManager>();

// ----------------------------------------
// CORS (Fix for Angular 4200)
// ----------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ----------------------------------------
// JWT Authentication
// ----------------------------------------
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
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role
    };
});

// Swagger
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
        Description = "Enter: Bearer {your token}"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});

// ----------------------------------------
// Build App
// ----------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS MUST COME BEFORE Auth
app.UseCors("AllowAngular");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
