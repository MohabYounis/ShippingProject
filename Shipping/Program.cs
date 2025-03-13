
using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.Repository;
using Shipping.Services;
using Shipping.UnitOfWorks;
using SHIPPING.Services;

namespace Shipping
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();



            //register context
            builder.Services.AddDbContext<ShippingContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("CS"));
            });
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ShippingContext>();

        

            //register automapper [add all profiles]
            builder.Services.AddAutoMapper(typeof(Program));


            //Register of Unit Of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Generic Repository
            builder.Services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));

            // Register Generic Service
            builder.Services.AddScoped(typeof(IServiceGeneric<>), typeof(ServiceGeneric<>));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
                app.MapOpenApi();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
