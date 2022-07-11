using ARPGCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.DB.Manager
{
    internal class ServerPropertyManager
    { 
        //得到服务器列表
        public List<ServerProperty> GetServerList()
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transcrition = session.BeginTransaction())
                {
                    var list = session.Query<ServerProperty>();
                    transcrition.Commit();
                    return list.ToList<ServerProperty>();
                }
            }
        }
    }
}
