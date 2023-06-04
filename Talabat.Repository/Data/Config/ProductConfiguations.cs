﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Config
{
    internal class ProductConfiguations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(P=>P.Name).HasMaxLength(100);
            builder.Property(P=>P.Price).HasColumnType("Decimal(18,2)");


            builder.HasOne(P => P.productBrand).WithMany();

            builder.HasOne(P=>P.productType).WithMany();

            
            
        }
    }
}
