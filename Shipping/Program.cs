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

            // إضافة SignalR للخدمات
            builder.Services.AddSignalR();

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

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            //Register of Unit Of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));

            // Register Identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ShippingContext>().AddDefaultTokenProviders(); ;
          
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            //register of SpecialShippingRateRepository
            builder.Services.AddScoped<ISpecialShippingRateRepository, SpecialShippingRateRepository>();

            //register of RolePermissionRepository
            builder.Services.AddScoped<IRolePermissinRepository, RolePermissinRepository>();

            //register of RolePermissionService
            builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();

            //role
            builder.Services.AddScoped<IApplicationRoleService, ApplicationRoleService>();

            // Register Generic Repository
            builder.Services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));

            // Register Generic Service
            builder.Services.AddScoped(typeof(IServiceGeneric<>), typeof(ServiceGeneric<>));

            // Register Delivery Service
            builder.Services.AddScoped<IDeliveryService, DeliveryService>();

            // Register RejectReason Service
            builder.Services.AddScoped<IRejectReasonService, RejectReasonService>();

            // Register Government Service
            builder.Services.AddScoped<IGovernmentService, GovernmentService>();

            //Register Merchant Service
            builder.Services.AddScoped<IMerchantService, MerchantService>();

            builder.Services.AddScoped<ISpecialShippingRateService, SpecialShippingRateService>();

            //Generate ResetToken Service
            builder.Services.AddScoped<IResetTokenService, ResetTokenService>();

            //Register City Service
            builder.Services.AddScoped<ICityService, CityService>();

            //Register Order Service
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
                app.UseSwagger(); 
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shipping API V1"));
               
            }

            // تكوين نقطة نهاية لـ SignalR
            app.MapHub<CityHub>("/cityHub");

            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}