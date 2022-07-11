using System;
using System.Collections.Generic;
using System.Text;

namespace ARPGCommon
{
    public enum OperationCode : byte
    {
        Login,
        GetServer,
        Register,
        Role,
        TaskDB,
        InventoryItemDB,
        SkillDB,
        Battle,
        Enemy,
        Boss
    }
}
