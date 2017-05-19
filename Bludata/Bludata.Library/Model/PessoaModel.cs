namespace Bludata.Library.Model
{
    using Bludata.Library.Enumerador;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Classe que representa uma pessoa cadastrada.
    /// </summary>
    [Table("bludata.pessoa")]
    public partial class PessoaModel
    {
        /// <summary>
        /// Construtor gerado apartir de database first.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PessoaModel()
        {
            Telefones = new HashSet<TelefoneModel>();
        }

        /// <summary>
        /// Id do objeto pessoa.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do objeto pessoa.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        /// <summary>
        /// CPF do objeto pessoa.
        /// </summary>
        [Required]
        [StringLength(15)]
        public string CPF { get; set; }

        /// <summary>
        /// Data de cadastro do objeto pessoa.
        /// </summary>
        public DateTime DataCadastro { get; set; }

        /// <summary>
        /// Data de nascimento do objeto pessoa.
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// RG do objeto pessoa (Somente disponível quando a UF for PR)
        /// </summary>
        [StringLength(15)]
        public string RG { get; set; }

        /// <summary>
        /// Unidade federativa em que a pessoa foi cadastrada.
        /// </summary>
        public UFEnum UF { get; set; }

        /// <summary>
        /// Lista de telefones vinculados a pessoa.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TelefoneModel> Telefones { get; set; }
    }
}
