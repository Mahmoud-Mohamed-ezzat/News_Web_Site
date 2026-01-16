using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using News_App;
using News_App.Interfaces;
using News_App.Models;
using News_App.Repository;
using News_App.services;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NewsContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Conn"));
}
);
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.IgnoreObsoleteActions();
    option.IgnoreObsoleteProperties();
    option.CustomSchemaIds(type => type.FullName);
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._- ";
    options.User.RequireUniqueEmail = true;
    // Remove the unique username requirement by not setting RequireUniqueUserName
    // options.User.RequireUniqueUserName = false; // This property doesn't exist in newer versions
    options.SignIn.RequireConfirmedEmail = true;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 9;
    options.Lockout.MaxFailedAccessAttempts = 3;
})
.AddEntityFrameworkStores<NewsContext>()
.AddDefaultTokenProviders();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme =
    option.DefaultForbidScheme =
    option.DefaultScheme =
    option.DefaultSignInScheme =
    option.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = false;
    option.SaveToken = false;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ClockSkew = TimeSpan.Zero //To make token notValidation after its time end

    };
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenServices>();
builder.Services.AddScoped<ImagesServices>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategory, CategoryRepository>();
builder.Services.AddScoped<INewsPage, NewsPageRepository>();
builder.Services.AddScoped<IPost, PostRepository>();
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
