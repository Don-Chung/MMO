using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float viewAngle = 50; //�������ӽǷ�Χ
    public float rotateSpeed = 1;
    public float attackDistance = 3;
    public float moveSpeed = 4;
    public float timeInterval = 1;
    public float timer = 0;
    public float[] attackArray;
    public int hp = 1000;
    public string guid;
    public int targetRoleId = -1;//��ʾ�������Ҫ������Ŀ��
    private GameObject targetGo;//��ʾҪ������Ŀ�����Ϸ����

    private Transform player;
    private Animation anim;
    private Rigidbody _rigidbody;
    private bool isAttacking = false;
    private Animation _animation;

    private Vector3 lastPosition = Vector3.zero;
    private Vector3 lastEulerAngles = Vector3.zero;

    private bool lastStand = false;
    private bool lastAttack01 = false;
    private bool lastAttack02 = false;
    private bool lastAttack03 = false;
    private bool lastDie = false;
    private bool lastHit = false;
    private bool lastWalk = false;
    private BossController bossController;

    private void Start()
    {
        _animation = GetComponent<Animation>();

        targetGo = GameController.Instance.GetPlayerByRoleID(targetRoleId);
        //player = TranscriptManager._instance.player.transform;
        player = targetGo.transform;

        TranscriptManage._instance.AddEnemy(this.gameObject);
        anim = this.GetComponent<Animation>();
        _rigidbody = this.GetComponent<Rigidbody>();
        BossHPProgressBar.Instance.Show(hp);

        bossController = GetComponent<BossController>();
        bossController.OnSyncBossAnimation += this.OnSyncBossAnimation;
        if (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster)
        {
            InvokeRepeating("CheckPositionAndRotation", 0, 1 / 30f);
            InvokeRepeating("CheckAnimation", 0, 1f / 30);
        }
    }

    private void Update()
    {
        if (GameController.Instance.battleType == BattleType.Person || (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster))
        {
            if (hp <= 0) return;
            if (_animation.IsPlaying("hit")) return;
            if (isAttacking) return;
            Vector3 playerPos = player.position;
            playerPos.y = transform.position.y;//��֤�нǲ���y���Ӱ��
            float angle = Vector3.Angle(playerPos - transform.position, transform.forward);
            if (angle < viewAngle / 2)
            {
                //�ڹ�����Ұ��
                //����ڲ��ڹ�������֮��
                float distance = Vector3.Distance(player.position, transform.position);
                if (distance < attackDistance)
                {
                    if (isAttacking == false)
                    {
                        anim.CrossFade("stand");
                        timer += Time.deltaTime;
                        if (timer > timeInterval)
                        {
                            timer = 0;
                            Attack();
                        }
                    }
                }
                else
                {
                    anim.CrossFade("walk");
                    _rigidbody.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                //������Ұ�⣬����ת��
                anim.CrossFade("walk");
                Quaternion targetRotation = Quaternion.LookRotation(playerPos - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }
        }

        
    }

    private int attackIndex = 0;
    void Attack()
    {
        attackIndex++;
        if (attackIndex == 4) attackIndex = 1;
        isAttacking = true;
        anim.CrossFade("attack0" + attackIndex.ToString());
        //if(attackIndex == 1)
        //{
        //    anim.CrossFade("attack01");
        //}else if(attackIndex == 2)
        //{
        //    anim.CrossFade("attack02");
        //}else if(attackIndex == 3)
        //{
        //    anim.CrossFade("attack02");
        //}
    }

    void backToStand()
    {
        isAttacking = false;
    }

    void PlayerAttack01()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if(distance < attackDistance)
        {
            player.SendMessage("TakeDamage", attackArray[0], SendMessageOptions.DontRequireReceiver);
        }
    }

    void PlayerAttack02()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDistance)
        {
            player.SendMessage("TakeDamage", attackArray[1], SendMessageOptions.DontRequireReceiver);
        }
    }

    void PlayerAttack03()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDistance)
        {
            player.SendMessage("TakeDamage", attackArray[2], SendMessageOptions.DontRequireReceiver);
        }
    }

    //�ܵ�����ʱ����
    //0�ܵ������˺�
    //1���˵ľ���
    //2���ո߶�
    void TakeDamage(string args)
    {
        if (hp <= 0) return;
        isAttacking = false;
        Combo._instance.ComboPlus();
        string[] proArray = args.Split(',');
        //
        int damage = int.Parse(proArray[0]);
        hp -= damage;
        BossHPProgressBar.Instance.UpdateShow(hp);
        //����bossѪ��
        //0�ܵ������˺�
        if (Random.Range(0, 10) == 9)
            _animation.Play("hit");
        //1���˵ľ��� 2���ո߶�
        float backDistance = float.Parse(proArray[1]);
        float jumpHeight = float.Parse(proArray[2]);
        if (Random.Range(0, 10) > 7)
        {
            iTween.MoveBy(this.gameObject,
                transform.InverseTransformDirection(TranscriptManage._instance.player.transform.forward) * backDistance
                + Vector3.up * jumpHeight,
                0.3f);
        }

        if (hp <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        _animation.Play("die");
        BossHPProgressBar.Instance.Hide();
        TranscriptManage._instance.RemoveEnemy(this.gameObject);
        GameController.Instance.OnBossDie();
    }

    void CheckPositionAndRotation()
    {
        Vector3 position = transform.position;
        Vector3 eulerAngles = transform.eulerAngles;
        if (position.x != lastPosition.x || position.y != lastPosition.y || position.z != lastPosition.z ||
            eulerAngles.x != lastEulerAngles.x || eulerAngles.y != lastEulerAngles.y ||
            eulerAngles.z != lastEulerAngles.z)
        {
            TranscriptManage._instance.AddBossToSync(this);
            lastPosition = position;
            lastEulerAngles = eulerAngles;
        }
    }

    void CheckAnimation()
    {
        if (lastStand != anim.IsPlaying("stand") || lastAttack01 != anim.IsPlaying("attack01") ||
            lastAttack02 != anim.IsPlaying("attack02") || lastAttack03 != anim.IsPlaying("attack03") ||
            lastDie != anim.IsPlaying("die") || lastHit != anim.IsPlaying("hit") ||
            lastWalk != anim.IsPlaying("walk"))
        {
            //ͬ��
            bossController.SyncBossAnimation(new BossAnimationModel() { attack01 = anim.IsPlaying("attack01"), attack02 = anim.IsPlaying("attack02"), attack03 = anim.IsPlaying("attack03"), die = anim.IsPlaying("die"), hit = anim.IsPlaying("hit"), stand = anim.IsPlaying("stand"), walk = anim.IsPlaying("walk") });

            lastStand = anim.IsPlaying("stand");
            lastAttack01 = anim.IsPlaying("attack01");
            lastAttack02 = anim.IsPlaying("attack02");
            lastAttack03 = anim.IsPlaying("attack03");
            lastDie = anim.IsPlaying("die");
            lastHit = anim.IsPlaying("hit");
            lastWalk = anim.IsPlaying("walk");
        }
    }

    public void OnSyncBossAnimation(BossAnimationModel model)
    {
        if (model.stand)
        {
            anim.Play("stand");
        }
        if (model.attack01)
        {
            anim.Play("attack01");
        }
        if (model.attack02)
        {
            anim.Play("attack02");
        }
        if (model.attack03)
        {
            anim.Play("attack03");
        }
        if (model.die)
        {
            anim.Play("die");
        }
        if (model.hit)
        {
            anim.Play("hit");
        }
        if (model.walk)
        {
            anim.Play("walk");
        }
    }
}
