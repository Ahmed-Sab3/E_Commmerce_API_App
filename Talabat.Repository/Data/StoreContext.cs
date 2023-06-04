﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Repository.Data.Config;

namespace Talabat.Repository.Data
{
    public class StoreContext:DbContext
    {

        public StoreContext(DbContextOptions<StoreContext> options):base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new ProductConfiguations());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 

            base.OnModelCreating(modelBuilder); 
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBrand> productBrands { get; set; }
        public DbSet<ProductType> productTypes { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

    }
}
