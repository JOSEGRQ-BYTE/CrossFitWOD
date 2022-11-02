using System.Diagnostics;
using System.Text;
using CrossFitWOD.Controllers;
using CrossFitWOD.Data;
using CrossFitWOD.Extensions;
using CrossFitWOD.Interfaces;
using CrossFitWOD.Models;
using CrossFitWOD.Repositories;
using HealthChecks.UI.Client;
using k8s.Models;
using MailKit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = "AllowedSpecificationsOrigins";
// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins,
                      builder => {
                          builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                      });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s => {
    s.SwaggerDoc("v1", new OpenApiInfo { Title = "Power - WOD", Version = "v1" });
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement
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
// Custom extention
//builder.Services.AddRepositories();


// Dependency Injection
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IWODRepository, WODRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// SQL Server Connection
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
builder.Services.AddIdentity<User, IdentityRole>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

// SQL Server Health Check
builder.Services.AddHealthChecks().AddCheck<SQLServerHealthCheck>("SQLDBConnectionCheck");

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


var app = builder.Build();



/*var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    Seed.SeedData(userManager, roleManager);
}*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}
//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();


DefaultFilesOptions options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("/index.html");
app.UseDefaultFiles(options);

//app.UseDefaultFiles(options);
app.UseStaticFiles();

app.UseRouting();
/*if (!string.IsNullOrEmpty(ServerConfig.FolderNotFoundFallbackPath))
{
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapFallbackToFile("/index.html");
    });
}*/


app.UseCors(allowedOrigins);

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("/{id}", "/index.html");
    endpoints.MapFallbackToFile("/", "/index.html");
});


app.MapControllers();
app.MapHealthChecks("/API/Health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

/*app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
    {
        context.Request.Path = "/index.html";
        context.Response.StatusCode = 200;
        await next();
    }
});*/

app.Run();
