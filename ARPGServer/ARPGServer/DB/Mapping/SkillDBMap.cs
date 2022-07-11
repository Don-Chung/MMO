using ARPGCommon.Model;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.DB.Mapping
{
    internal class SkillDBMap : ClassMap<SkillDB>
    {
        public SkillDBMap()
        {
            Id(x => x.ID).Column("id");
            Map(x => x.SkillID).Column("skillid");
            References(x => x.Role).Column("roleid");
            Map(x => x.Level).Column("level");
            Table("skilldb");
        }
    }
}
