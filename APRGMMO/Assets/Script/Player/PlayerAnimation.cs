using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private PlayerAttack playerAttack;

    private Player player;
    private BattleController battleController;
    private bool isSyncPlayerAnimation = false;//表示是否需要同步动画

    private void Start()
    {
        player = GetComponent<Player>();
        if (GameController.Instance.battleType == BattleType.Team && player.roleID == PhotonEngine.Instance.role.ID)
        {//当前角色属于当前客户端 
            battleController = GameController.Instance.GetComponent<BattleController>();
            isSyncPlayerAnimation = true;
        }
        anim = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    public void OnAttackButtonClick(bool isPress, PosType posType)
    {
        if (playerAttack.hp <= 0) return;
        if(posType == PosType.Basic && isPress)
        {
            anim.SetTrigger("Attack");
            if (isSyncPlayerAnimation)
            {
                battleController.SyncPlayerAnimation(new PlayerAnimationModel() { attack = true });
            }
        }
        else
        {
            anim.SetBool("Skill" + ((int)posType).ToString(), isPress);
            if (isSyncPlayerAnimation)
            {
                //技能按下时
                if (isPress)
                {
                    switch ((int)posType)
                    {
                        case 1:
                            battleController.SyncPlayerAnimation(new PlayerAnimationModel() { skill1 = true });
                            break;
                        case 2:
                            battleController.SyncPlayerAnimation(new PlayerAnimationModel() { skill2 = true });
                            break;
                        case 3:
                            battleController.SyncPlayerAnimation(new PlayerAnimationModel() { skill3 = true });
                            break;

                    }
                }
                else
                {
                    battleController.SyncPlayerAnimation(new PlayerAnimationModel());
                }

            }
        }
    }

    public void SyncAnimation(PlayerAnimationModel model)
    {
        if (model.attack)
        {
            anim.SetTrigger("Attack");
        }
        else if (model.die)
        {
            anim.SetTrigger("Die");
        }
        else if (model.takeDamage)
        {
            anim.SetTrigger("TakeDamage");
        }
        anim.SetBool("Skill1", model.skill1);
        anim.SetBool("Skill2", model.skill2);
        anim.SetBool("Skill3", model.skill3);
    }
}
