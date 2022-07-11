using ARPGCommon.Model;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnSyncTaskCompleteEvent();
public delegate void OnGetRoleEvent(List<Role> roleList);
public delegate void OnAddRoleEvent(Role role);
public delegate void OnSelectRoleEvent();
public delegate void OnGetTaskDBListEvent(List<TaskDB> list);
public delegate void OnAddTaskDBEvent(TaskDB taskDB);
public delegate void OnUpdateTaskDBEvent();
public delegate void OnGetInventoryItemDBListEvent(List<InventoryItemDB> list);
public delegate void OnAddInventoryItemDBEvent(InventoryItemDB itemDB);
public delegate void OnUpdateInventoryItemDBEvent();
public delegate void OnUpdateInventoryItemDBListEvent();
public delegate void OnUpgradeEquipEvent();
public delegate void OnGetSkillDBListEvent(List<SkillDB> list);
public delegate void OnAddSkillDBEvent(SkillDB skillDB);
public delegate void OnUpdateSkillDBEvent();
public delegate void OnUpgradeSkillDBEvent(SkillDB skillDB);
public delegate void OnSyncSkillCompleteEvent();
public delegate void OnPlayerHpChangeEvent(int hp);

public delegate void OnGetTeamEvent(List<Role> rolelist, int masterRoleID);//组队成功

public delegate void OnWaitingTeamEvent();//等待组队中

public delegate void OnCancelTeamEvent();//取消组队的响应

public delegate void OnSyncPositionAndRotationEvent(int roleid, Vector3 position, Vector3 eulerAngles);

public delegate void OnSyncMoveAnimationEvent(int roleid, PlayerMoveAnimationModel model);

public delegate void OnCreateEnemyEvent(CreateEnemyModel model);

public delegate void OnSyncEnemyPositionRotationEvent(EnemyPositionModel model);

public delegate void OnSyncEnemyAnimationEvent(EnemyAnimationModel model);

public delegate void OnSyncPlayerAnimationEvent(int roleId, PlayerAnimationModel model);

public delegate void OnGameStateChangeEvent(GameStateModel model);

public delegate void OnSyncBossAnimationEvent(BossAnimationModel model);