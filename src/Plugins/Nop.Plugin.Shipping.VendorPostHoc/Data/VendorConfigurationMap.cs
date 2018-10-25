using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Shipping.VendorPostHoc.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Shipping.VendorPostHoc.Data
{
    public class VendorConfigurationMap : NopEntityTypeConfiguration<VendorConfiguration>
    {
        public override void Configure(EntityTypeBuilder<VendorConfiguration> builder)
        {
            builder.ToTable(nameof(VendorConfiguration));
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.VendorId);
            builder.Property(e => e.ShippingCost).HasColumnType("decimal(18, 4)");
        }
    }
}
