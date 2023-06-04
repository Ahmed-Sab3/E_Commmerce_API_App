using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Repository.Data.Config
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, Address => Address.WithOwner());

            builder.Property(O => O.status)
                .HasConversion(
                    OStatus=>OStatus.ToString(),
                    OStatus => (OrderStatus) Enum.Parse(typeof(OrderStatus), OStatus)
                );

            builder.Property(O => O.Subtotal)
                .HasColumnType("decimal(18,2)");
        }
    }
}
