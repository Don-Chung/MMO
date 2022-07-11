using ARPGCommon.Model;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.DB.Mapping
{
    public class InventoryItemMap : ClassMap<InventoryItemDB>
    {
        public InventoryItemMap()
        {
            Id(x => x.ID).Column("id");
            Map(x => x.InventoryID).Column("inventoryid");
            Map(x => x.Level).Column("level");
            Map(x => x.Count).Column("count");
            Map(x => x.IsDressed).Column("isdressed");
            References(x => x.Role).Column("roleid");
            Table("inventoryitemdb");
        }
    }
}
