namespace Bludata.Library.Model
{
    using System.Data.Entity;

    /// <summary>
    /// Classe context que contém a configuração do banco de dados.
    /// </summary>
    public partial class Context : DbContext
    {
        /// <summary>
        /// Construtor da classe contex.
        /// </summary>
        public Context()
            : base("name=Context")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Tabela Pessoa.
        /// </summary>
        public virtual DbSet<PessoaModel> Pessoa { get; set; }

        /// <summary>
        /// Tabela Telefone.
        /// </summary>
        public virtual DbSet<TelefoneModel> Telefone { get; set; }

        /// <summary>
        /// Inicializa o context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PessoaModel>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaModel>()
                .Property(e => e.CPF)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaModel>()
                .Property(e => e.RG)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaModel>()
                .HasMany(e => e.Telefones)
                .WithRequired(e => e.pessoa)
                .HasForeignKey(e => e.IdPessoa);

            modelBuilder.Entity<TelefoneModel>()
                .Property(e => e.Telefone)
                .IsUnicode(false);
        }
    }
}
