using Bludata.DAO.Factory;
using Bludata.Library.Model;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bludata.DAO.Repository
{
    public class PessoaRepository
    {
        public static void Alterar(PessoaModel model)
        {
            using (ISession session = FluentyFactory.AbrirSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Recupera os telefones cadastrados para o usuário em questão
                        var telfonesDbB = session.Query<TelefoneModel>().Where(x => x.IdPessoa == model.Id).ToList();
                        
                        // Array de telefones vindos da tela do usuário
                        string[] telefoneAdd = model.Telefones.Select(x => x.Telefone).ToArray();
                        
                        // Deleta os telefones que foram removidos pelo usuário.
                        var telefonesDeletar = telfonesDbB.Where(x => !telefoneAdd.Contains(x.Telefone)).ToList();
                        foreach (TelefoneModel telefone in telefonesDeletar)
                        {
                            session.Delete(telefone);
                            telfonesDbB.Remove(telefone);
                        }
                        
                        // Adiciona os novos telefones que foram adicionados pelo usuário
                        model.Telefones = model.Telefones.Where(x => !telfonesDbB.Where(y => y.Telefone == x.Telefone).Any()).ToList();
                        //foreach (TelefoneModel telefone in model.Telefones)
                        //{
                        //    session.Save(telefone);
                        //}
                        

                        session.Update(model);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!transaction.WasCommitted)
                        {
                            transaction.Rollback();
                        }

                        throw ex;
                    }
                }
            }
        }

        public static PessoaModel Consultar(int id)
        {
            using (ISession session = FluentyFactory.AbrirSession())
            {
                return session.Get<PessoaModel>(id);
            }
        }

        public static List<PessoaModel> ConsultarTodos()
        {
            using (ISession session = FluentyFactory.AbrirSession())
            {
                return session.Query<PessoaModel>().Fetch(x => x.Telefones).ToList();
            }
        }

        public static void Deletar(int id)
        {
            using (ISession session = FluentyFactory.AbrirSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        var telefones = session.Query<TelefoneModel>().Where(x => x.IdPessoa == id).ToList();
                        foreach (TelefoneModel telefone in telefones)
                        {
                            session.Delete(telefone);
                        }
                        session.Flush();

                        PessoaModel model = session.Get<PessoaModel>(id);
                        session.Delete(model);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!transaction.WasCommitted)
                        {
                            transaction.Rollback();
                        }

                        throw ex;
                    }
                }
            }
        }

        public static void Inserir(PessoaModel model)
        {
            using (ISession session = FluentyFactory.AbrirSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        var telefones = model.Telefones;
                        model.Telefones = null;

                        int pessoaId = (int)session.Save(model);

                        foreach (TelefoneModel telefone in telefones)
                        {
                            telefone.IdPessoa = pessoaId;
                            session.Save(telefone);
                        
                            session.Clear();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!transaction.WasCommitted)
                        {
                            transaction.Rollback();
                        }

                        throw ex;
                    }
                }
            }
        }
    }
}
