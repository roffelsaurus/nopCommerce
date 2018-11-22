using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Payments.StripeConnect.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect.Data
{
    public class StripeOrderChargeMap : NopEntityTypeConfiguration<StripeOrderCharge>
    {
        public override void Configure(EntityTypeBuilder<StripeOrderCharge> builder)
        {
            builder.ToTable(nameof(StripeOrderCharge));
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.OrderGuid);
        }
    }
}
