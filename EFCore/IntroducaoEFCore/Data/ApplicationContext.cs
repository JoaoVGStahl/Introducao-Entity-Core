using IEFCore.Data.Configurations;
using IEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace IEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        private readonly ILoggerFactory _logger = LoggerFactory.Create(l => l.AddConsole());

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Data Source=DESKTOP-LD0IN04\\DELLSERVER;Initial Catalog=EFCore;Integrated Security=true;"
                , p => p.EnableRetryOnFailure(
                    maxRetryCount: 3, 
                    maxRetryDelay: TimeSpan.FromSeconds(5), 
                    errorNumbersToAdd: null).MigrationsHistoryTable("EF_Core_Migration_History"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            /*
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            */
            MapearPropriedadesEsquecidas(modelBuilder);

        }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var propertyEntity = entity.GetProperties().Where(p => p.ClrType == typeof(string));
                foreach (var props in propertyEntity)
                {
                    if(string.IsNullOrEmpty(props.GetColumnType()) || !props.GetMaxLength().HasValue)
                    {
                        //props.SetMaxLength(100);

                        props.SetColumnType("Varchar(100)");
                    }
                }
            }
        }
    }
}
