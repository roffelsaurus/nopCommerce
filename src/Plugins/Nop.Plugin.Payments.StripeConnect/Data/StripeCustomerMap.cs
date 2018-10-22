using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Payments.StripeConnect.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect.Data
{
    public class StripeCustomerMap : NopEntityTypeConfiguration<StripeCustomer>
    {
        public override void Configure(EntityTypeBuilder<StripeCustomer> builder)
        {
            builder.ToTable(nameof(StripeCustomer));
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.CustomerId);
        }
    }
}
