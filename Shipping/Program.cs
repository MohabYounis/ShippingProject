using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shipping.Configration;
using Shipping.Controllers;
using Shipping.DTOs;
using Shipping.ImodelRepository;
using Shipping.modelRepository;
using Shipping.Models;
using Shipping.Repository;
using Shipping.Repository.ImodelRepository;
using Shipping.Repository.modelRepository;
using Shipping.Services;
using Shipping.Services.IModelService;
using Shipping.Services.ModelService;
using Shipping.SignalRHubs;
using Shipping.UnitOfWorks;
using SHIPPING.Services;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Shipping.ImodelRepository;
using Shipping.modelRepository;
using System.Reflection;
using Shipping.SignalRHubs;


namespace Shipping
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            // Add OpenAPI (Swagger) support
            builder.Services.AddOpenApi();
            //Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Shipping API",
                    Version = "v1"
                });

                // 🔐 تعريف JWT Bearer
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                // 🔗 ربطه بكل الـ endpoints   
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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


            //register context
            builder.Services.AddDbContext<ShippingContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("CS"));
            });
          
            // Register Identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ShippingContext>().AddDefaultTokenProviders(); ;
          
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));
            builder.Services.AddScoped(typeof(IServiceGeneric<>), typeof(ServiceGeneric<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IMerchantService, MerchantService>();
            builder.Services.AddScoped<IDeliveryService, DeliveryService>();
            builder.Services.AddScoped<ISpecialShippingRateRepository, SpecialShippingRateRepository>();
            builder.Services.AddScoped<IRolePermissinRepository, RolePermissinRepository>();
            builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
            builder.Services.AddScoped<IApplicationRoleService, ApplicationRoleService>();
            builder.Services.AddScoped<IApplicationRoleRepository, ApplicationRoleRepository>();
            builder.Services.AddScoped<IRejectReasonService, RejectReasonService>();
            builder.Services.AddScoped<IGovernmentService, GovernmentService>();
            builder.Services.AddScoped<ISpecialShippingRateService, SpecialShippingRateService>();

            //Generate ResetToken Service
            builder.Services.AddScoped<IResetTokenService, ResetTokenService>();

            //Register City Service

            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IApplicationRoleService, ApplicationRoleService>();
            builder.Services.AddScoped<IWeightPricingService, WeightPricingService>();

            //jwt
            var jwtOptions= builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
            builder.Services.AddSingleton(jwtOptions);
            builder.Services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer=jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),//حولي signkey الي array of byte 
                    };
                }).AddJwtBearer("ResetToken", options =>
                {
                    // Reset Password JWT
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });


            //Email smtp
            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
            builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();


            // For Profile Image
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; // السماح برفع ملفات حتى 100 ميجابايت
            });


            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                            builder.WithOrigins("http://localhost:4200")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            //add memory cashe
            builder.Services.AddMemoryCache();

            builder.Services.AddSignalR();

            // Add logging
            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
            });

            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
                app.UseSwagger(); 
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shipping API V1"));
               
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            // Enable CORS
            app.MapHub<OrderHub>("/orderHub");

            // Enable CORS
            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}