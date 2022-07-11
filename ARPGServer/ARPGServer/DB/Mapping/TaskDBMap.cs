using ARPGCommon.Model;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.DB.Mapping
{
    internal class TaskDBMap : ClassMap<TaskDB>
    {
        public TaskDBMap()
        {
            Id(x => x.ID).Column("id");
            Map(x => x.LastUpdateTime).Column("lastupdatetime");
            Map(x => x.State).Column("state");
            Map(x => x.TaskID).Column("taskid");
            Map(x => x.Type).Column("type");
            References(x => x.Role).Column("roleid");
            Table("taskdb");
        }
    }
}
