using ARPGCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.DB.Manager
{
    public class TaskDBManager
    {
        public List<TaskDB> GetTaskDBList(Role role)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    var res = session.QueryOver<TaskDB>().Where(x => x.Role == role);
                    transcation.Commit();
                    return (List<TaskDB>)res.List();
                }
            }
        }

        public void AddTaskDB(TaskDB taskDB)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    taskDB.LastUpdateTime = new DateTime();
                    session.Save(taskDB);
                    transcation.Commit();
                }
            }
        }

        public void UpdateTaskDB(TaskDB taskDB)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    taskDB.LastUpdateTime = new DateTime();
                    session.Update(taskDB);
                    transcation.Commit();
                }
            }
        }
    }
}
