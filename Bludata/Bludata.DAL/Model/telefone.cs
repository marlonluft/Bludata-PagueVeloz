namespace Bludata.DAL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("bludata.telefone")]
    public partial class telefone
    {
        public int Id { get; set; }

        public int IdPessoa { get; set; }

        [Column("Telefone")]
        [Required]
        [StringLength(15)]
        public string Telefone { get; set; }

        public virtual pessoa pessoa { get; set; }
    }
}
