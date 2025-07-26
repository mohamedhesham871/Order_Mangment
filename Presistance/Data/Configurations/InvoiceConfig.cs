using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class InvoiceConfig : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
          builder.HasOne<Order>()
                 .WithOne()
                 .HasForeignKey<Invoice>(i => i.OrderId)
                 .OnDelete(DeleteBehavior.Cascade);
     
         builder.Property(i => i.InvoiceDate).HasDefaultValue(DateTimeOffset.Now);
        }

    }
}
