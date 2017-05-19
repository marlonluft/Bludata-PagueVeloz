namespace Bludata.DAL.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }

        public virtual DbSet<pessoa> pessoa { get; set; }
        public virtual DbSet<telefone> telefone { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<pessoa>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<pessoa>()
                .Property(e => e.CPF)
                .IsUnicode(false);

            modelBuilder.Entity<pessoa>()
                .Property(e => e.RG)
                .IsUnicode(false);

            modelBuilder.Entity<pessoa>()
                .HasMany(e => e.telefone)
                .WithRequired(e => e.pessoa)
                .HasForeignKey(e => e.IdPessoa);

            modelBuilder.Entity<telefone>()
                .Property(e => e.Telefone)
                .IsUnicode(false);
        }
    }
}
