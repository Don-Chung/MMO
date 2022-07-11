using ARPGCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.DB.Manager
{
    public class InventoryItemDBManager
    {
        public List<InventoryItemDB> GetInventoryItemDB(Role role)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    var res = session.QueryOver<InventoryItemDB>().Where(x => x.Role == role);
                    transcation.Commit();
                    return (List<InventoryItemDB>)res.List<InventoryItemDB>();
                }
            }
        }

        public void AddInventoryItemDB(InventoryItemDB itemDB)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    session.Save(itemDB);
                    transcation.Commit();
                }
            }
        }

        internal void UpdateInventoryItemDB(InventoryItemDB itemDB)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    session.Update(itemDB);
                    transcation.Commit();
                }
            }
        }

        public void UpdateInventoryItemDBList(List<InventoryItemDB> list)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    foreach (var itemDB in list)
                    {
                        session.Update(itemDB);
                    }
                    transcation.Commit();
                }
            }
        }

        internal void UpgradeEquip(InventoryItemDB itemDB4, Role role)
        {
            using (var session = NHibernateHelper.GetSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    session.Update(itemDB4);
                    session.Update(role);
                    transcation.Commit();
                }
            }
        }
    }
}
