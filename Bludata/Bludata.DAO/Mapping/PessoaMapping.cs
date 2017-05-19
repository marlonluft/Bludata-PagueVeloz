using FluentNHibernate.Mapping;
using Bludata.Library.Model;
using Bludata.Library.Interface;
using System;
using Bludata.Library.Enumerador;

namespace Bludata.DAO.Mapping
{
    public class PessoaMapping : ClassMap<PessoaModel>
    {
        public PessoaMapping()
        {
            Id(x => x.Id).Not.Nullable();
            Map(x => x.CPF).Not.Nullable();
            Map(x => x.DataCadastro).Not.Nullable();
            Map(x => x.DataNascimento).Not.Nullable();
            Map(x => x.Nome).Not.Nullable();
            Map(x => x.RG).Not.Nullable();
            Map(x => x.UF).CustomType(typeof (UFEnum)).Not.Nullable();
            //HasMany(x => x.Telefones).Table("telefone").KeyColumn("IdPessoa").AsBag().Not.LazyLoad();
            HasMany(x => x.Telefones).Table("telefone").KeyColumn("IdPessoa").AsBag().Not.LazyLoad().Inverse().Cascade.AllDeleteOrphan().KeyColumn("IdPessoa");
            Table("pessoa");
        }
    }
}
