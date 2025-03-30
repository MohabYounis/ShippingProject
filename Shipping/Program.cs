using Microsoft.EntityFrameworkCore;
using Shipping.DTOs;
using Shipping.Models;
using Shipping.Repository;
using Shipping.Services;
using Shipping.Services.IModelService;
using Shipping.Services.ModelService;
using Shipping.UnitOfWorks;
using SHIPPING.Services;
using Microsoft.OpenApi.Models;
using Shipping.Controllers;
using Shipping.Repository.ImodelRepository;
using Shipping.Repository.modelRepository;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Shipping.ImodelRepository;
using Shipping.modelRepository;

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shipping API", Version = "v1" });
            });

            //register context
            builder.Services.AddDbContext<ShippingContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("CS"));
            });
          
            // Register Identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ShippingContext>();
          
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            //Register of Unit Of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
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
            builder.Services.AddScoped<IMerchantService, MerchantService>(); //Register Merchant Service
            builder.Services.AddScoped<ISpecialShippingRateService, SpecialShippingRateService>();
            //Register City Service
            builder.Services.AddScoped<ICityService, CityService>();
            //Register Order Service
            builder.Services.AddScoped<IOrderService, OrderService>();
            // Register Generic Service
            builder.Services.AddScoped<GeneralResponse>();

            builder.Services.AddScoped<IApplicationRoleService, ApplicationRoleService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));
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
                });


            // For Profile Image
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; // السماح برفع ملفات حتى 100 ميجابايت
            });


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); 
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shipping API V1"));
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}