using Domin.Models;
using Domin.Models.identitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>                        
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(builder => builder.Id);
            builder.Property(o => o.OrderDate)
                   .HasDefaultValueSql("getdate()"); // Default value for OrderDate
            builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
            builder.HasOne<Customer>()
                   .WithMany()
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade); 

        }
    }
}
