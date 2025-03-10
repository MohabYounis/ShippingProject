using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Shipping.Models
{
    public class ShippingContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Government> Governments { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShippingType> ShippingTypes { get; set; }
        public virtual DbSet<SpecialShippingRate> SpecialShippingRates { get; set; }
        public virtual DbSet<WeightPricing> WeightPricings { get; set; }

        public ShippingContext() : base() { }

        public ShippingContext (DbContextOptions<ShippingContext> optios) : base(optios) { }
    }
}
