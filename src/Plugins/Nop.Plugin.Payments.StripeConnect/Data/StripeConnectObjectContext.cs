using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nop.Core;
using Nop.Data;
using Nop.Data.Extensions;
using Nop.Plugin.Payments.StripeConnect.Domain;

namespace Nop.Plugin.Payments.StripeConnect.Data
{
    /// <summary>
    /// Represents plugin object context
    /// </summary>
    public class StripeConnectObjectContext : DbContext, IDbContext
    {

        public StripeConnectObjectContext(DbContextOptions<StripeConnectObjectContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StripeCustomerMap());
            modelBuilder.ApplyConfiguration(new StripeOrderChargeMap());
            base.OnModelCreating(modelBuilder);
        }
        
        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
        
        public virtual string GenerateCreateScript()
        {
            return this.Database.GenerateCreateScript();
        }
        
        public virtual IQueryable<TQuery> QueryFromSql<TQuery>(string sql) where TQuery : class
        {
            throw new NotImplementedException();
        }
        
        public virtual IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public virtual int ExecuteSqlCommand(RawSqlString sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                var result = this.Database.ExecuteSqlCommand(sql, parameters);
                transaction.Commit();

                return result;
            }
        }
        
        public virtual void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }
        
        public void Install()
        {
            this.ExecuteSqlScript(this.GenerateCreateScript());
        }
        
        public void Uninstall()
        {
            this.DropPluginTable(nameof(StripeCustomer));
            this.DropPluginTable(nameof(StripeOrderCharge));
        }
    }
}