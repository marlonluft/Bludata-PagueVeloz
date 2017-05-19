using Bludata.Library.Enumerador;
using Bludata.Library.Model;
using Bludata.Library.Resource;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bludata.ViewModel
{
    /// <summary>
    /// View model de apresentação da classe PessoaModel.
    /// </summary>
    public class PessoaViewModel
    {
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public PessoaViewModel()
        {
            DataCadastro = DateTime.Now;
        }

        /// <summary>
        /// Construtor que converte um objeto PessoaModel em PessoaViewModel.
        /// </summary>
        /// <param name="model"></param>
        public PessoaViewModel(PessoaModel model)
        {
            this.CPF = model.CPF;
            this.DataCadastro = model.DataCadastro;
            this.DataNascimento = model.DataNascimento.ToShortDateString();
            this.Id = model.Id;
            this.Nome = model.Nome;
            this.RG = model.RG;
            this.Telefones = model.Telefones.Select(x => x.Telefone).ToArray();
            this.UF = model.UF;
        }

        /// <summary>
        /// Campo ID da pessoa.
        /// </summary>
        [Key()]
        public int Id { get; set; }

        /// <summary>
        /// Campo nome da pessoa.
        /// </summary>
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceName = "Msg_NomeTamanho", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Nome", ResourceType = typeof(Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Msg_NomeObrigatorio", ErrorMessageResourceType = typeof(Resource))]
        public string Nome { get; set; }

        /// <summary>
        /// Campo CPF da pessoa.
        /// </summary>
        [MaxLength(15, ErrorMessageResourceName = "Msg_TamanhoMaximo", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Text)]
        [Display(Name = "CPF", ResourceType = typeof(Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Msg_CPFObrigatorio", ErrorMessageResourceType = typeof(Resource))]
        public string CPF { get; set; }
        
        /// <summary>
        /// Data de nascimento da pessoa.
        /// </summary>
        [Display(Name = "DataNascimento", ResourceType = typeof(Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Msg_DataNascimentoObrigatorio", ErrorMessageResourceType = typeof(Resource))]
        public string DataNascimento { get; set; }

        /// <summary>
        /// Data de cadastro da pessoa.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "DataCadastro", ResourceType = typeof(Resource), AutoGenerateField = false)]
        public DateTime DataCadastro { get; set; }

        /// <summary>
        /// RG da pessoa.
        /// </summary>
        [MaxLength(15, ErrorMessageResourceName = "Msg_TamanhoMaximo", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Text)]
        [Display(Name = "RG", ResourceType = typeof(Resource), AutoGenerateField = false)]
        public string RG { get; set; }
        
        /// <summary>
        /// Unidade federativa da pessoa.
        /// </summary>
        [Display(Name = "UF", ResourceType = typeof(Resource), AutoGenerateField = false)]
        public UFEnum UF { get; set; }

        /// <summary>
        /// Conjunto de telefone concatenados da pessoa.
        /// </summary>
        [Display(Name = "Telefone", ResourceType = typeof(Resource), AutoGenerateField = false)]
        public string Telefone
        {
            get
            {
                return String.Join(" / ", this.Telefones);
            }
        }
        
        /// <summary>
        /// Conjunto de telefones da pessoa.
        /// </summary>
        public string[] Telefones
        {
            get;
            set;
        }
    }
}