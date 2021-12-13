using IEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IEFCore.Data.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Nome).HasColumnType("Varchar(100)").IsRequired();
            builder.Property(c => c.Telefone).HasColumnType("Char(11)");
            builder.Property(c => c.CEP).HasColumnType("Char(8)").IsRequired();
            builder.Property(c => c.Estado).HasColumnType("Char(2)").IsRequired();
            builder.Property(c => c.Cidade).HasMaxLength(60).IsRequired();

            builder.HasIndex(i => i.Telefone).HasName("idx_cliente_telefone");
        }
    }
}
