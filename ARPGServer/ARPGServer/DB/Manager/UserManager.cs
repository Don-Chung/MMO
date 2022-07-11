using ARPGCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.DB.Manager
{
    public class UserManager
    {
        public User GetUserByUsername(string username)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var res = session.QueryOver<User>().Where(x => x.Username == username);
                    transaction.Commit();
                    if(res.List() != null && res.List().Count() > 0)
                    {
                        return res.List()[0];
                    }
                    return null;
                }
            }
        }

        public void AddUser(User user)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }
    }
}
