using System;
using System.Collections.Generic;
using System.Text;

namespace ARPGCommon
{
    public enum ParameterCode : byte
    {
        ServerList,
        User,
        RoleList,
        Role,
        SubCode,
        OperationCode,
        TaskDB,
        TaskDBList,
        InventoryItemDBList,
        InventoryItemDB,
        SkillDBList,
        SkillDB,
        MasterRoleID,
        Position,//位置
        EulerAngles,//旋转
        RoleID,//角色的id，表示是更新的哪一个客户端
        IsMove,
        PlayerMoveAnimationModel,
        CreateEnemyModel,
        EnemyPositionModel,
        EnemyAnimationModel,
        PlayerAnimationModel,
        GameStateModel,
        BossAnimationModel
    }
}
