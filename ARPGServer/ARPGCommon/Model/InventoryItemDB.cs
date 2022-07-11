using System;
using System.Collections.Generic;
using System.Text;

namespace ARPGCommon.Model
{
    public class InventoryItemDB
    {
        public virtual int ID { get; set; }
        public virtual int InventoryID { get; set; }
        public virtual int Level { get; set; }
        public virtual int Count { get; set; }
        public virtual bool IsDressed { get; set; }
        public virtual Role Role { get; set; }
    }
}
