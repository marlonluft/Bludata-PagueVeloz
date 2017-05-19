using Bludata.Library.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Bludata.Library.DAL
{
    /// <summary>
    /// Classe responsável por manter pessoa no banco de dados.
    /// </summary>
    public class PessoaDAL
    {
        /// <summary>
        /// Realiza a consulta de todas as pessoas cadastradas no banco de dados.
        /// </summary>
        /// <returns>Lista de objetos PessoaModel.</returns>
        public static List<PessoaModel> ConsultarTodos()
        {
            using (Context context = new Context())
            {
                return context.Pessoa.Include("Telefones").ToList();
            }
        }

        /// <summary>
        /// Realiza a inserção de uma pessoa e suas dependências no banco de dados.
        /// </summary>
        /// <param name="model"></param>
        public static void Inserir(PessoaModel model)
        {
            using (Context context = new Context())
            {
                context.Pessoa.Add(model);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Realiza a consulta de uma pessoa no banco de dados pelo campo id.
        /// </summary>
        /// <param name="id">Id para busca no banco de dados.</param>
        /// <returns>Objeto pessoa.</returns>
        public static PessoaModel Consultar(int id)
        {
            using (Context context = new Context())
            {
                return context.Pessoa.Include("Telefones").Where(x => x.Id == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// Realiza a alteração de uma pessoa e suas dependências no banco de dados.
        /// </summary>
        /// <param name="model">Objeto pessoa alterado.</param>
        public static void Alterar(PessoaModel model)
        {
            using (Context context = new Context())
            {
                // Recupera somente os números de telefones vindos da tela do usuário
                string[] telefonesArray = model.Telefones.Select(x => x.Telefone).ToArray();

                // Recupera os telefones do banco de dados que tenham relação com a pessoa.
                var listaTelefone = context.Telefone.Where(x => x.IdPessoa == model.Id).ToList();

                // Remove os telefones removidos pelo usuário
                var telefoneRemover =  listaTelefone.Where(x => !telefonesArray.Contains(x.Telefone)).ToList();                
                foreach (TelefoneModel telefone in telefoneRemover)
                {
                    context.Telefone.Remove(telefone);
                    listaTelefone.Remove(telefone);
                }

                // Recupera os telefones do banco de dados restantes
                telefonesArray = listaTelefone.Select(x => x.Telefone).ToArray();

                // Adiciona os novos telefones 
                var telefoneAdicionar = model.Telefones.Where(x => !telefonesArray.Contains(x.Telefone)).ToList();
                foreach (TelefoneModel telefone in telefoneAdicionar)
                {
                    context.Telefone.Add(telefone);
                }
                
                // Remove a lista de telefones para não ocorrer problemas com o entity.
                model.Telefones = null;

                // Altera a pessoa no banco de dados
                context.Pessoa.Attach(model);
                context.Entry(model).State = EntityState.Modified;

                // Salva toda a operação
                context.SaveChanges();

            }
        }

        /// <summary>
        /// Realiza a exclusão da pessoa e suas dependencias no banco de dados.
        /// </summary>
        /// <param name="id">Id da pessoa que será removida.</param>
        public static void Excluir(int id)
        {
            using (Context contex = new Context())
            {
                var model = contex.Pessoa.Include("Telefones").Where(x => x.Id == id).FirstOrDefault();

                if (model == null)
                {
                    throw new Exception(Resource.Resource.Msg_NenhumaPessoaEncotradaConformeIdEncontrado);
                }
                else
                {
                    contex.Pessoa.Remove(model);
                    contex.Entry(model).State = EntityState.Deleted;
                    contex.SaveChanges();
                }
            }
        }
    }
}
