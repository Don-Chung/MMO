using ARPGCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.DB.Manager
{
    public class SkillDBManager
    {
        public void Add(SkillDB skillDB)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(skillDB);
                    transaction.Commit();
                }
            }
        }

        public void Update(SkillDB skillDB)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(skillDB);
                    transaction.Commit();
                }
            }
        }

        public void Upgrade(SkillDB skillDB, Role role)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(skillDB);
                    session.Update(role);
                    transaction.Commit();
                }
            }
        }

        public List<SkillDB> Get(Role role)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var res = session.QueryOver<SkillDB>().Where(x => x.Role == role);
                    transaction.Commit();
                    return (List<SkillDB>)res.List<SkillDB>();
                }
            }
        }
    }
}
