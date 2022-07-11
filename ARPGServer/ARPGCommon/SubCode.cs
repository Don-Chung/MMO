using System;
using System.Collections.Generic;
using System.Text;

namespace ARPGCommon
{
    public enum SubCode
    {
        GetRole,
        AddRole,
        SelectRole,
        UpdateRole,
        AddTaskDB,
        UpdateTaskDB,
        GetTaskDB,
        GetInventoryItemDB,
        AddInventoryItemDB,
        UpdateInventoryItemDB,
        UpdateInventoryItemDBList,
        UpgradeEquip,
        Add,
        Update,
        Get,
        Upgrade,
        SendTeam,    //组队的请求
        CancelTeam,
        GetTeam,   //组队成功
        SyncPositionAndRotation,//同步位置和旋转
        SyncMoveAnimation,//同步移动动画
        CreateEnemy,//创建敌人 产生敌人
        SyncAnimation,
        SendGameState,
        SyncBossAnimation
    }
}
