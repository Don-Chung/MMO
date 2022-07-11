using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float distanceAttackForward = 2f;
    public float distanceAttackAround = 4f;
    public int[] damageArray = new int[] {20, 30, 30, 30};
    public enum AttackRange
    {
        Forward,
        Around
    }
    public int hp = 1000;
    private Animator anim;
    private GameObject hudTextGameObject;
    private HUDText hudText;
    private Transform damageShowPoint;

    private Player player;
    private BattleController battleController;
    private bool isSyncPlayerAnimation = false;//��ʾ�Ƿ���Ҫͬ������

    private void Start()
    {
        player = GetComponent<Player>();
        if (GameController.Instance.battleType == BattleType.Team && player.roleID == PhotonEngine.Instance.role.ID)//��ǰ��ɫ���ڵ�ǰ�ͻ���
        {
            battleController = GameController.Instance.GetComponent<BattleController>();
            isSyncPlayerAnimation = true;
        }
        hp = PlayerInfo._instance.HP;

        anim = GetComponent<Animator>();
        damageShowPoint = transform.Find("damageShowPoint");
        hudTextGameObject = HpBarManager._instance.GetHudText(damageShowPoint.gameObject);
        hudText = hudTextGameObject.GetComponent<HUDText>();
    }

    //0 nomal skill1 skill2 skill3
    //1 
    //2 sound
    //3 move forward ����ʱĳЩ֡��Ҫ��ǰλ�ƣ��Ƶ���
    //4 jump height //���˻���
    void Attack(string args)
    {
        string[] proArray = args.Split(',');
        //2 play sound
        string soundName = proArray[2];
        SoundManager._instance.Play(soundName);
        //3 move forward
        float moveForward = float.Parse(proArray[3]);
        if(moveForward > 0.1f)
        {
            iTween.MoveBy(this.gameObject, Vector3.forward * moveForward, 0.3f);
        }
        string posType = proArray[0];
        if (posType == "normal")
        {
            ArrayList array = GetEnemyInAttackRange(AttackRange.Forward);
            foreach(GameObject go in array)
            {
                go.SendMessage("TakeDamage", damageArray[0].ToString() + "," + proArray[3] + "," + proArray[4]);
            }
        }
    }

    void SkillAttack(string args)
    {
        string[] proArray = args.Split(',');
        string posType = proArray[0];
        if (posType == "skill1")
        {
            ArrayList array = GetEnemyInAttackRange(AttackRange.Around);
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", damageArray[1].ToString() + "," + proArray[3] + "," + proArray[4]);
            }
        }
        else if (posType == "skill2")
        {
            ArrayList array = GetEnemyInAttackRange(AttackRange.Around);
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", damageArray[2].ToString() + "," + proArray[3] + "," + proArray[4]);
            }
        }
        else if (posType == "skill3")
        {
            ArrayList array = GetEnemyInAttackRange(AttackRange.Around);
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", damageArray[3].ToString() + "," + proArray[3] + "," + proArray[4]);
            }
        }
    }

    //�õ��ڹ�����Χ�ڵ���
    ArrayList GetEnemyInAttackRange(AttackRange attackRange)
    {
        ArrayList arrayList = new ArrayList();
        //��ͨ����
        if(attackRange == AttackRange.Forward)
        {
            foreach(GameObject go in TranscriptManage._instance.GetEnemyList())
            {
                //������������תΪplayer�ľֲ�����
                //Vector3 pos = this.transform.InverseTransformDirection(go.transform.position);
                //print(pos);
                Vector3 pos = go.transform.position - this.transform.position;
                if(pos.z > -0.57f)
                {
                    float distance = Vector3.Distance(this.transform.position, go.transform.position);
                    //float distance = Vector3.Distance(Vector3.zero, pos);
                    if (distance < distanceAttackForward)
                    {
                        arrayList.Add(go);
                    }
                }
            }
        }
        else  //��Χ����
        {
            foreach (GameObject go in TranscriptManage._instance.GetEnemyList())
            {
                float distance = Vector3.Distance(this.transform.position, go.transform.position);
                if (distance < distanceAttackAround)
                {
                    arrayList.Add(go);
                }
            }
        }
        return arrayList;
    }

    void TakeDamage(int damage)
    {
        if (this.hp <= 0) return;
        this.hp -= damage;
        if (OnPlayerHpChange != null)
            OnPlayerHpChange(hp);
        //�����ܵ������Ķ���
        int random = Random.Range(0, 100);
        if(random < damage)
        {
            anim.SetTrigger("TakeDamage");
            //ͬ��
            if (isSyncPlayerAnimation)
            {
                PlayerAnimationModel model = new PlayerAnimationModel() { takeDamage = true };
                battleController.SyncPlayerAnimation(model);
            }
        }
        //��ʾѪ���ļ���
        hudText.Add("-" + damage.ToString(), Color.red, 0.3f);
        //��Ļ��Ѫ�����Ч��ʾ
        BloodScreen.Instance.Show();
        if(hp <= 0)
        {
            hp = 0;
            anim.SetTrigger("Die");
            if (isSyncPlayerAnimation)
            {
                PlayerAnimationModel model = new PlayerAnimationModel() { die = true };
                battleController.SyncPlayerAnimation(model);
            }
            GameController.Instance.OnPlayerDie(GetComponent<Player>().roleID);
        }
    }

    public event OnPlayerHpChangeEvent OnPlayerHpChange;
}
