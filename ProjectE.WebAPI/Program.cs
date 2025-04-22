using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProjectE.Business.Abstract;
using ProjectE.Business.Concrete;
using ProjectE.Business.Helpers;
using ProjectE.Business.Settings;
using ProjectE.DataAccess.Context;
using ProjectE.DataAccess.Settings;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// JWT Settings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<TokenGenerator>();
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

// JWT Authentication
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// MongoDB Settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbContext>();

// DI
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICompanyAuthService, CompanyAuthManager>();
builder.Services.AddScoped<ICompanyService, CompanyManager>();
builder.Services.AddScoped<IOfferService, OfferManager>();




// Swagger with JWT Support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Token'ýnýzý girin. Format: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // <-- EKLENDÝ
app.UseAuthorization();

app.MapControllers();

app.Run();
