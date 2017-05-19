using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bludata.DAO.Factory
{
    public class FluentyFactory
    {
        public static string connectionString
        {
            get
            {
                var conn = ConfigurationManager.ConnectionStrings["MySqlConnection"];
                return  (conn != null) ? conn.ConnectionString : string.Empty;
            }
        }        

        private static ISessionFactory instance;

        public static ISessionFactory Instance()
        {
            if (instance == null)
            {
                IPersistenceConfigurer configure = MySQLConfiguration.Standard.ConnectionString(connectionString);

                var configMap = Fluently
                    .Configure()
                    .Database(configure)
                    .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Mapping.PessoaMapping>());
                
                instance = configMap.BuildSessionFactory();
            }

            return instance;
        }

        public static ISession AbrirSession()
        {
            return Instance().OpenSession();
        }
    }
}
