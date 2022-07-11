using ARPGCommon.Model;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.DB.Mapping
{
    internal class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.ID).Column("id");
            Map(x => x.Username).Column("username");
            Map(x => x.Password).Column("password");
            Table("user");
        }
    }
}
