using Bludata.Library.DAL;
using Bludata.Library.Enumerador;
using Bludata.Library.Model;
using Bludata.Library.Resource;
using Bludata.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Bludata.Controllers
{
    /// <summary>
    /// Controle responsável por manter Pessoas.
    /// </summary>
    public class PessoaController : Controller
    {
        /// <summary>
        /// Recupera a informção setada em Cookie no navegador que informa a unidade federativa do usuário.
        /// </summary>
        private UFEnum UnidadeFederativa
        {
            get
            {
                var unidadeFederativa = Request.Cookies["UNIDADE_FEDERATIVA"];

                if (unidadeFederativa != null)
                {
                    return (UFEnum)Enum.Parse(typeof(UFEnum), unidadeFederativa.Value);
                }

                return UFEnum.PR;
            }
        }

        /// <summary>
        /// Retorna a tela principal com todas as pessoas cadastradas no sistema.
        /// </summary>
        /// <returns>View de listagem de pessoas.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            List<PessoaViewModel> lista = new List<PessoaViewModel>();

            try
            {                
                lista = PessoaDAL.ConsultarTodos()
                    .Select(x => new PessoaViewModel(x))
                    .ToList();
            }
            catch (Exception ex)
            {
                TempData["ErroMensagem"] = string.Format(Resource.Msg_ErroException, ex.Message);
            }

            return View(lista);
        }

        /// <summary>
        /// Retorna a tela de cadastro de pessoas, passando na viewbag UnidadeFederativa a UF do usuário.
        /// </summary>
        /// <returns>View de cadastro de pessoas.</returns>
        [HttpGet]
        public ActionResult Cadastrar()
        {
            try
            {
                ViewBag.UnidadeFederativa = UnidadeFederativa;
            }
            catch (Exception ex)
            {
                TempData["ErroMensagem"] = string.Format(Resource.Msg_ErroException, ex.Message);
            }

            return View();
        }

        /// <summary>
        /// Método de retorno da tela de cadastro;
        /// Realiza a validação e inserção da pessoa para o banco de dados.
        /// </summary>
        /// <param name="model">Dados informado pelo cliente no cadastro de uma nova pessoa.</param>
        /// <returns>Retorna para página inicial em caso de sucesso.</returns>
        [HttpPost]
        public ActionResult Cadastrar(PessoaViewModel model)
        {
            try
            {
                DateTime dtNascimento = new DateTime();

                if (model != null)
                {
                    UFEnum uf = UnidadeFederativa;
                    
                    if (!DateTime.TryParse(model.DataNascimento, out dtNascimento) && dtNascimento < DateTime.Now)
                    {
                        ModelState.AddModelError("DataNascimentoString", Resource.Msg_DataInvalida);
                    }
                    else
                    {                        
                        if (uf == UFEnum.PR && (DateTime.Now.Year - dtNascimento.Year) < 18)
                        {
                            ModelState.AddModelError(string.Empty, Resource.Msg_NaoPermitidoCadastrarPessoaIdadeInferiorDezoito);
                        }
                    }

                    if (uf == UFEnum.SC && string.IsNullOrWhiteSpace(model.RG))
                    {
                        ModelState.AddModelError(string.Empty, Resource.Msg_ObrigatorioInformaRGUnidadeFederativaSC);
                    }

                    // Remover campos de telefone vazio
                    model.Telefones = model.Telefones.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    if (model.Telefones.Length == 0)
                    {
                        ModelState.AddModelError("Telefone", Resource.Msg_FavorAdicionarAoMenosUmTelefone);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, Resource.Msg_OsDadosRecebidosPeloServidorEstaoVazios);
                }

                if (ModelState.IsValid)
                {
                    PessoaModel pessoa = new PessoaModel();
                    pessoa.Nome = model.Nome;
                    pessoa.RG = model.RG;
                    pessoa.UF = this.UnidadeFederativa;
                    pessoa.CPF = model.CPF;
                    pessoa.DataCadastro = model.DataCadastro;
                    pessoa.DataNascimento = dtNascimento;

                    pessoa.Telefones = new List<TelefoneModel>();

                    for (int i = 0; i < model.Telefones.Length; i++)
                    {
                        TelefoneModel telefone = new TelefoneModel();
                        telefone.Telefone = model.Telefones[i];

                        pessoa.Telefones.Add(telefone);
                    }

                    PessoaDAL.Inserir(pessoa);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format(Resource.Msg_ErroException, ex.Message));
            }

            ViewBag.UnidadeFederativa = UnidadeFederativa;

            return View(model);
        }

        /// <summary>
        /// Método que retorna a tela de alteração para a pessoa com o id especificado por parâmetro.
        /// </summary>
        /// <param name="id">Id da pessoa a ser alterada.</param>
        /// <returns>View Alterar.</returns>
        [HttpGet]
        public ActionResult Alterar(int id)
        {
            try
            {
                PessoaModel pessoa = PessoaDAL.Consultar(id);

                if (pessoa != null)
                {
                    PessoaViewModel model = new PessoaViewModel(pessoa);
                    return View(model);
                }
                else
                {
                    TempData["ErroMensagem"] = Resource.Msg_NenhumaPessoaEncotradaConformeIdEncontrado;
                }
            }
            catch (Exception ex)
            {
                TempData["ErroMensagem"] = string.Format(Resource.Msg_ErroException, ex.Message);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Método de retorno da tela de alteração, Realiza a validação e alteração da pessoa.
        /// </summary>
        /// <param name="model">Dados da pessoa a ser alterada.</param>
        /// <returns>Retorna para página inicial em caso de sucesso.</returns>
        [HttpPost]
        public ActionResult Alterar(PessoaViewModel model)
        {
            try
            {
                DateTime dtNascimento = new DateTime();

                if (model != null)
                {
                    UFEnum uf = UnidadeFederativa;

                    if (!DateTime.TryParse(model.DataNascimento, out dtNascimento) && dtNascimento < DateTime.Now)
                    {
                        ModelState.AddModelError("DataNascimentoString", Resource.Msg_DataInvalida);
                    }
                    else
                    {
                        if (uf == UFEnum.PR && (DateTime.Now.Year - dtNascimento.Year) < 18)
                        {
                            ModelState.AddModelError(string.Empty, Resource.Msg_NaoPermitidoCadastrarPessoaIdadeInferiorDezoito);
                        }
                    }

                    if (uf == UFEnum.SC && string.IsNullOrWhiteSpace(model.RG))
                    {
                        ModelState.AddModelError(string.Empty, Resource.Msg_ObrigatorioInformaRGUnidadeFederativaSC);
                    }

                    // Remover campos de telefone vazio
                    model.Telefones = model.Telefones.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    if (model.Telefones.Length == 0)
                    {
                        ModelState.AddModelError("Telefone", Resource.Msg_FavorAdicionarAoMenosUmTelefone);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, Resource.Msg_OsDadosRecebidosPeloServidorEstaoVazios);
                }

                if (ModelState.IsValid)
                {
                    PessoaModel pessoa = new PessoaModel();
                    pessoa.Id = model.Id;
                    pessoa.Nome = model.Nome;
                    pessoa.RG = model.RG;
                    pessoa.UF = this.UnidadeFederativa;
                    pessoa.CPF = model.CPF;
                    pessoa.DataCadastro = model.DataCadastro;
                    pessoa.DataNascimento = dtNascimento;

                    pessoa.Telefones = new List<TelefoneModel>();

                    for (int i = 0; i < model.Telefones.Length; i++)
                    {
                        TelefoneModel telefone = new TelefoneModel();
                        telefone.Telefone = model.Telefones[i];
                        telefone.IdPessoa = model.Id;

                        pessoa.Telefones.Add(telefone);
                    }

                    PessoaDAL.Alterar(pessoa);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format(Resource.Msg_ErroException, ex.Message));
            }

            return View(model);
        }

        /// <summary>
        /// Método de exclução de pessoa.
        /// </summary>
        /// <param name="id">Id da pessoa a ser removida.</param>
        /// <returns>Mensagem de sucesso/erro.</returns>
        [HttpPost]
        public JsonResult Deletar(int id)
        {
            bool sucesso = true;
            string mensagem = string.Empty;

            try
            {
                PessoaDAL.Excluir(id);

                mensagem = Resource.Msg_PessoaDeletadaComSucesso;
            }
            catch (Exception ex)
            {
                sucesso = false;
                mensagem = string.Format(Resource.Msg_FalhaAoDeletarAPessoa, ex.Message);
            }

            return Json(new
            {
                Mensagem = mensagem,
                Sucesso = sucesso
            });
        }
    }
}
