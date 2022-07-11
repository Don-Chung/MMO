using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animation anim;
    public int hp = 200;
    public float speed = 2;
    public int attackRate = 2; //����Ƶ��
    private float attackTimer = 0;
    public float attackDistance = 1.6f;
    public float damage = 20; //������
    public float downSpeed = 1f;
    public string guid = "";//GUID,ÿ�����˵�Ψһ��ʶ
    public int targetRoleId = -1;//��ʾ�������Ҫ������Ŀ��
    private GameObject targetGo;//��ʾҪ������Ŀ�����Ϸ����
    private int hpTotal = 0;
    private float downDistance = 0;
    private float distance = 0;
    private CharacterController cc;
    private GameObject hpBarGameObject;
    private UISlider hpBarSlider;
    private GameObject hudTextGameObject;
    private HUDText hudText;
    private Vector3 lastPosition = Vector3.zero;
    private Vector3 lastEulerAngles = Vector3.zero;

    private bool lastIsIdle = true;
    private bool lastIsWalk = false;
    private bool lastIsAttack = false;
    private bool lastIsTakeDamage = false;
    private bool lastIsDie = false;

    private void Start()
    {
        targetGo = GameController.Instance.GetPlayerByRoleID(targetRoleId);
        TranscriptManage._instance.AddEnemy(this.gameObject);
        hpTotal = hp;
        anim = GetComponent<Animation>();
        cc = GetComponent<CharacterController>();
        InvokeRepeating("CalcDistance", 0, 0.1f);  //1�����ʮ��

        Transform hpBarPoint = transform.Find("HpBarPoint");
        hpBarGameObject = HpBarManager._instance.GetHpBar(hpBarPoint.gameObject);
        hpBarSlider = hpBarGameObject.transform.Find("Bg").GetComponent<UISlider>();

        hudTextGameObject = HpBarManager._instance.GetHudText(hpBarPoint.gameObject);
        hudText = hudTextGameObject.GetComponent<HUDText>();

        if (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster)
        {
            InvokeRepeating("CheckPositionAndRotation", 0, 1 / 30f);
            InvokeRepeating("CheckAnimation", 0, 1 / 30f);
        }
    }

    private void Update()
    {
        if (hp <= 0) {
            //����
            downDistance += downSpeed * Time.deltaTime;
            transform.Translate(-transform.up * downSpeed * Time.deltaTime);
            if(downDistance > 4f)
            {
                Destroy(this.gameObject);
            }
            return;
        }
        if (GameController.Instance.battleType == BattleType.Person || (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster))
        {
            if (distance < attackDistance)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer > attackRate)
                {
                    Transform player = TranscriptManage._instance.player.transform;
                    Vector3 targetPos = player.position;
                    targetPos.y = transform.position.y;
                    transform.LookAt(targetPos);
                    //���й���
                    anim.Play("attack01");
                    attackTimer = 0;
                }
                if (!anim.IsPlaying("attack01"))
                {
                    anim.CrossFade("idle");
                }
            }
            else
            {
                anim.Play("walk");
                Move();
            }
        }
            
    }

    void Move()
    {
        Transform player = TranscriptManage._instance.player.transform;
        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
        cc.SimpleMove(transform.forward * speed);
    }

    void CalcDistance()
    {
        Transform player = TranscriptManage._instance.player.transform;
        distance = Vector3.Distance(player.position, transform.position);
    }

    //�ܵ�����ʱ����
    //0�ܵ������˺�
    //1���˵ľ���
    //2���ո߶�
    void TakeDamage(string args)
    {
        if (hp <= 0) return;
        Combo._instance.ComboPlus();
        string[] proArray = args.Split(',');
        //
        int damage = int.Parse(proArray[0]);
        hp -= damage;
        hpBarSlider.value = (float)hp / hpTotal;
        hudText.Add("-" + damage.ToString(), Color.red, 0.3f); 
        //0�ܵ������˺�
        anim.Play("takedamage");
        SoundManager._instance.Play("Hurt");
        //1���˵ľ��� 2���ո߶�
        float backDistance = float.Parse(proArray[1]);
        float jumpHeight = float.Parse(proArray[2]);
        //print(transform.InverseTransformDirection
        //    (TranscriptManage._instance.player.transform.
        //    forward));
        iTween.MoveBy(this.gameObject,
            transform.InverseTransformDirection
            (TranscriptManage._instance.player.transform.
            forward)*backDistance + Vector3.up
            * jumpHeight, 0.3f);

        if(hp <= 0)
        {
            Dead();
        }
    }

    public void Attack()
    {
        //Transform player = TranscriptManage._instance.player.transform;
        Transform player = targetGo.transform;
        distance = Vector3.Distance(player.position, transform.position);
        if(distance < attackDistance)
        {
            player.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    //����
    void Dead()
    {
        TranscriptManage._instance.RemoveEnemy(this.gameObject);
        this.GetComponent<CharacterController>().enabled = false;
        Destroy(hpBarGameObject);
        Destroy(hudTextGameObject);
        //��һ��������ʽ����������
        //�ڶ���������ʽ������Ч��
        int random = Random.Range(0, 10);
        if(random <= 7)
        {
            anim.Play("die");
        }
        else
        {
            this.GetComponentInChildren<MeshExploder>().Explode();
            this.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
    }

    void CheckPositionAndRotation()
    {
        Vector3 position = transform.position;
        Vector3 eulerAngles = transform.eulerAngles;
        if (position.x != lastPosition.x || position.y != lastPosition.y || position.z != lastPosition.z ||
            eulerAngles.x != lastEulerAngles.x || eulerAngles.y != lastEulerAngles.y ||
            eulerAngles.z != lastEulerAngles.z)
        {
            TranscriptManage._instance.AddEnemyToSync(this);
            lastPosition = position;
            lastEulerAngles = eulerAngles;
        }
    }

    void CheckAnimation()
    {
        if (lastIsAttack != anim.IsPlaying("attack01") || lastIsDie != anim.IsPlaying("die") || lastIsIdle != anim.IsPlaying("idle") || lastIsTakeDamage != anim.IsPlaying("takedamage") || lastIsWalk != anim.IsPlaying("walk"))
        {
            TranscriptManage._instance.AddEnemyToSyncAnimation(this);// �������ݵ����������� ͳһ���ж����ĸ���
            lastIsAttack = anim.IsPlaying("attack01");
            lastIsDie = anim.IsPlaying("die");
            lastIsIdle = anim.IsPlaying("idle");
            lastIsTakeDamage = anim.IsPlaying("takedamage");
            lastIsWalk = anim.IsPlaying("walk");
        }
    }
}
