using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Shipping.Models
{
    public class ShippingContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Government> Governments { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShippingType> ShippingTypes { get; set; }
        public virtual DbSet<SpecialShippingRate> SpecialShippingRates { get; set; }
        public virtual DbSet<WeightPricing> WeightPricings { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Setting> ShippingToVillages { get; set; }
        public virtual DbSet<DeliveryGovernment> DeliveryGovernments { get; set; }
        public virtual DbSet<BranchMerchant> BranchMerchants { get; set; }
        public virtual DbSet<RejectedOrder> RejectedOrders { get; set; }

        public ShippingContext() : base() { }
        public ShippingContext (DbContextOptions<ShippingContext> options) : base(options) { }
    }
}
