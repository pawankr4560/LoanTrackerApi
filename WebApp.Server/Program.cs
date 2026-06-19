using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;
using WebApp.Data;
using WebApp.Data.Entity;
using WebApp.Data.Repository;
using WebApp.Data.SeedData;
using WebApp.Service.Auth;
using WebApp.Service.Message;
using WebApp.Service.Order;
using WebApp.Service.Product;
using WebApp.Service.Transaction;

namespace WebApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // Controllers
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.MaxDepth = 128;
                });

            builder.Services.AddEndpointsApiExplorer();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // DbContext
            var connectionString = configuration.GetConnectionString("Default");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Connection string 'Default' not found.");
            }

            builder.Services.AddDbContext<WebAppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Seeder
            builder.Services.AddScoped<SeedData>();

            // Http Context
            builder.Services.AddHttpContextAccessor();

            // AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // MSG91
            builder.Services.AddHttpClient("MSG91Client", client =>
            {
                var msg91 = configuration.GetSection("MSG91");

                var baseUrl = msg91["BaseUrl"];

                if (!string.IsNullOrEmpty(baseUrl))
                {
                    client.BaseAddress = new Uri(baseUrl);
                }

                client.DefaultRequestHeaders.Add("authkey", msg91["AuthKey"] ?? "");
                client.DefaultRequestHeaders.Add("accept", "application/json");
            });

            // Identity
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<WebAppDbContext>()
            .AddDefaultTokenProviders();

            // JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            configuration["Jwt:Key"]
                            ?? "SuperSecretBackupKeyForDeployment123456"))
                };
            });

            // Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Web App API",
                        Version = "v1"
                    });

                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter JWT Token"
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            // Generic Repository
            builder.Services.AddTransient<
                IGenericRepository<StripeCustomer>,
                GenericRepository<StripeCustomer>>();

            // Services
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<IProductService, WebApp.Service.Product.ProductService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<ILoanService, LoanService>();
            builder.Services.AddTransient<ILoanPaymentService, LoanPaymentService>();
            builder.Services.AddTransient<ILoanEMIScheduleService, LoanEMIScheduleService>();
            builder.Services.AddTransient<IStripeService, StripeService>();

            builder.Services.AddScoped<IMessageService, MessageService>();

            // Stripe Services
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddScoped<ChargeService>();
            builder.Services.AddScoped<Stripe.ProductService>();
            builder.Services.AddScoped<PriceService>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<CardService>();

            builder.Services.AddHttpClient();
            builder.Services.AddOptions();

            // Stripe Configuration
            var stripeSecretKey = configuration["Stripe:Secret_Key"];

            if (!string.IsNullOrWhiteSpace(stripeSecretKey))
            {
                StripeConfiguration.ApiKey = stripeSecretKey;
            }

            var app = builder.Build();

            // Development Exception Page
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Database Seeder
            try
            {
                using var scope = app.Services.CreateScope();

                var seeder = scope.ServiceProvider.GetRequiredService<SeedData>();

                seeder.SeedAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Seeding Failed: {ex}");
            }

            // Swagger
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web App API V1");
                c.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("EnableCORS");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.MapGet("/", () => Results.Redirect("/swagger"));

            app.MapGet("/test", () => "API Running");

            app.Run();
        }
    }
}