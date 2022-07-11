using ARPGCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.DB.Manager
{
    public class RoleManager
    {
        public List<Role> GetRoleListByUser(User user)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var res = session.QueryOver<Role>().Where(role => role.User == user);
                    transaction.Commit();
                    return (List<Role>)res.List<Role>();
                }
            }
        }

        public void AddRole(Role role)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(role);
                    transaction.Commit();
                }
            }
        }

        public void UpdateRole(Role role)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(role);
                    transaction.Commit();
                }
            }
        }
    }
}
