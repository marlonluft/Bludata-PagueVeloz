namespace Bludata.Library.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Classe que representa uma telefone vinculado ao uma pessoa cadastrada.
    /// </summary>
    [Table("bludata.telefone")]
    public partial class TelefoneModel
    {
        /// <summary>
        /// Id do objeto telefone.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID da pessoa que o objeto telefone é vinculado.
        /// </summary>
        public int IdPessoa { get; set; }

        /// <summary>
        /// Telefone do objeto telefone.
        /// </summary>
        [Column("Telefone")]
        [Required]
        [StringLength(16)]
        public string Telefone { get; set; }

        /// <summary>
        /// Classe da pessoa que o objeto telefone é vinculado.
        /// </summary>
        public virtual PessoaModel pessoa { get; set; }
    }
}
