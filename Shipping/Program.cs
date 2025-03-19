
using Microsoft.AspNetCore.Builder;
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


namespace Shipping
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            // Cancel Filter Above Actions and depend on ModelState.IsValid
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ShippingContext>();

            //------------------
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            //register automapper [add all profiles]
            builder.Services.AddAutoMapper(typeof(Program));

            
            //Register of Unit Of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Generic Repository
            builder.Services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));

            // Register Generic Service
            builder.Services.AddScoped(typeof(IServiceGeneric<>), typeof(ServiceGeneric<>));
            //Register Delivery Service
            builder.Services.AddScoped<IDeliveryService, DeliveryService>();
            // Register Generic Service
            builder.Services.AddScoped<GeneralResponse>();

                        builder.Services.AddEndpointsApiExplorer();

                        builder.Services.AddSwaggerGen();

            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); 
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shipping API V1"));
            }



            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }
    }
}
