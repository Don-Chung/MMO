using System;
using System.Collections.Generic;
using System.Text;

namespace ARPGCommon.Model
{
    public class TaskDB
    {
        public virtual int ID { get; set; }
        public virtual Role Role { get; set; }
        public virtual int TaskID { get; set; }
        public virtual int State { get; set; }
        public virtual int Type { get; set; }
        public virtual DateTime LastUpdateTime { get; set; }
    }
}
