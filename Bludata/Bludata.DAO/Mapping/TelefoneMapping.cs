using FluentNHibernate.Mapping;
using Bludata.Library.Model;

namespace Bludata.DAO.Mapping
{
    public class TelefoneMapping : ClassMap<TelefoneModel>
    {
        public TelefoneMapping()
        {
            Id(x => x.Id).Not.Nullable();
            Map(x => x.IdPessoa).Not.Nullable();
            Map(x => x.Telefone).Not.Nullable();
            Table("telefone");
        }
    }
}
