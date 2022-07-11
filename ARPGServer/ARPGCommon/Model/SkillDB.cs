using System;
using System.Collections.Generic;
using System.Text;

namespace ARPGCommon.Model
{
    public class SkillDB
    {
        public virtual int ID { get; set; }
        public virtual int SkillID { get; set; }
        public virtual Role Role { get; set; }
        public virtual int Level { get; set; }
    }
}
