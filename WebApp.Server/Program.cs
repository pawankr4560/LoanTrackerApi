using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using Stripe.Climate;
using System.Text;
using WebApp.Data;
using WebApp.Data.Entity;
using WebApp.Data.Repository;
using WebApp.Data.SeedData;
using WebApp.Service.Auth;
using WebApp.Service.Middleware;
using WebApp.Service.Product;
using WebApp.Service.Stripe;

namespace WebApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMvc();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.MaxDepth = 128;
            });


            builder.Services.AddScoped<SeedData>();

            // Add services for identity
            var configuration = builder.Configuration;

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();

                });

            });

            builder.Services.AddDbContext<WebAppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });
            builder.Services.AddIdentity<User, IdentityRole>(
                option =>
                {
                    option.Password.RequireDigit = false;
                    option.Password.RequiredLength = 5;
                    option.Password.RequireLowercase = false;
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = false;
                    option.SignIn.RequireConfirmedEmail = true;
                    option.SignIn.RequireConfirmedEmail = true;
                }
            ).AddEntityFrameworkStores<WebAppDbContext>()
            .AddDefaultTokenProviders();

            //Initialize configuration
           
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            //Register repository
            builder.Services.AddTransient<IGenericRepository<StripeCustomer>, GenericRepository<StripeCustomer>>();

            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<IProductService, Service.Product.ProductService>();
            builder.Services.AddTransient<OrderService, OrderService>();
            builder.Services.AddTransient<IStripeService, StripeService>();
          
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddScoped<ChargeService>();
            builder.Services.AddScoped<Stripe.ProductService>();
            builder.Services.AddScoped<Stripe.PriceService>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<CardService>();
            StripeConfiguration.ApiKey = configuration.GetSection("Stripe:Secret_Key").Value.ToString();

            builder.Services.AddOptions();
            builder.Services.AddHttpClient();

            //Add services for jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };

            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Web App", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
            });

            

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();


            var seeder = new Seeder(app);
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("EnableCORS");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<WebAppMiddleware>();
            app.MapControllers();
            

            app.Run();

        }
        public class Seeder
        {
            public Seeder(WebApplication app)
            {
                using (var scope = app.Services.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<SeedData>();
                    seeder?.SeedAsync().Wait();
                }
            }
        }
    }
}
