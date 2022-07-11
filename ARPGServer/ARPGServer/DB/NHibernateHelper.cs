using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.DB
{
    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory = null;

        private static void InitSessionFactory()
        {
            MySQLConfiguration configuration = MySQLConfiguration.Standard.ConnectionString(db => db.Server("localhost")
            .Database("arpgserver").Username("root").Password("zly@elf"));
            sessionFactory = Fluently.Configure().Database(configuration).Mappings(x => x.FluentMappings
            .AddFromAssemblyOf<NHibernateHelper>()).BuildSessionFactory();
        }

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    InitSessionFactory();
                }
                return sessionFactory;
            }
        }

        public static ISession GetSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
