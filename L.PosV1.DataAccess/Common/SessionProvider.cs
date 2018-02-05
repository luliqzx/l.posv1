using L.PosV1.DataAccess.Map;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace L.PosV1.DataAccess.Common
{
    public class CollectionSessionFactory
    {
        public virtual string CSN { get; set; }
        public virtual ISessionFactory SessionFactory { get; set; }
    }

    public interface ISessionProvider
    {
        IList<CollectionSessionFactory> CollectionSessionFactories { get; set; }
        ISessionFactory GetSessionFactory(string CSN = "");
    }

    public class SessionProvider : ISessionProvider
    {
        public IList<CollectionSessionFactory> CollectionSessionFactories
        {
            get;
            set;
        }

        public ISessionFactory GetSessionFactory(string CSN = "")
        {
            if (string.IsNullOrWhiteSpace(CSN))
            {
                return CollectionSessionFactories.FirstOrDefault().SessionFactory;
            }
            return CollectionSessionFactories.FirstOrDefault(x => x.CSN == CSN).SessionFactory;
        }

        public SessionProvider()
        {
            if (CollectionSessionFactories == null)
            {
                CollectionSessionFactories = new List<CollectionSessionFactory>();
            }

            string CSN1 = "mssqlserverConn";
            //string CSN2 = "mssqlserverConn2";

            CollectionSessionFactory varsf = CollectionSessionFactories.FirstOrDefault(x => x.CSN == CSN1);
            if (varsf == null)
            {
                varsf = new CollectionSessionFactory { CSN = CSN1, SessionFactory = CreateSessionFactory(CSN1) };
                CollectionSessionFactories.Add(varsf);
            }

            //varsf = CollectionSessionFactories.FirstOrDefault(x => x.CSN == CSN2);
            //if (varsf == null)
            //{
            //    varsf = new CollectionSessionFactory { CSN = CSN2, SessionFactory = CreateSessionFactory(CSN2) };
            //    CollectionSessionFactories.Add(varsf);
            //}
        }

        public ISessionFactory CreateSessionFactory(string name)
        {
            FluentConfiguration fc = Fluently.Configure();
            if ("mssqlserverConn" == name)
            {
                fc.Database(CreateMSSqlDbConfig())
                       .Mappings(m => m.FluentMappings
                           .AddFromAssemblyOf<UserMap>()
                           //.AddFromAssemblyOf<CustomerMap>()
                           //.AddFromAssemblyOf<CustomerAddressMap>()
                           //.AddFromAssemblyOf<CountryMap>()
                           //.AddFromAssemblyOf<AddressMap>()
                           );
            }
            //else if ("mssqlserverConn2" == name)
            //{
            //    fc.Database(CreateMSSqlDbConfig2())
            //           .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CurrencyMap>());
            //}

            fc.ExposeConfiguration(UpdateSchema)
            .ExposeConfiguration(cfg => cfg.Properties.Add("use_proxy_validator", "false"))
            .ExposeConfiguration(config =>
            {
                config.SetInterceptor(new SqlStatementInterceptor());
                config.SetProperty(NHibernate.Cfg.Environment.SqlExceptionConverter,
                    typeof(SqlExceptionConverter).AssemblyQualifiedName);
            });
            return fc.BuildSessionFactory();
        }

        // Returns our database configuration
        private static MsSqlConfiguration CreateMSSqlDbConfig()
        {
            MsSqlConfiguration MsSqlConfiguration = MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("mssqlserverConn"))
            #region Debug
#if debug
                .ShowSql()
#endif
;
            #endregion
            return MsSqlConfiguration;
        }

        // Returns our database configuration
        private static MsSqlConfiguration CreateMSSqlDbConfig2()
        {
            MsSqlConfiguration MsSqlConfiguration = MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("mssqlserverConn2"))
            #region Debug
#if debug
                .ShowSql()
#endif
;
            #endregion
            return MsSqlConfiguration;
        }

        private static void UpdateSchema(Configuration cfg)
        {
            new SchemaUpdate(cfg)
                .Execute(false, true);
        }

    }
}
