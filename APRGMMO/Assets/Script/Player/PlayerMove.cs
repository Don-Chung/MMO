using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float velocity = 8;
    public bool isCanControl = true;
    private Rigidbody _rigidbody;
    private Animator anim;
    private PlayerAttack playerAttack;
    private Vector3 lastPosition = Vector3.zero;
    private Vector3 lastEulerAngles = Vector3.zero;
    private bool isMove = false;
    private DateTime lastUpdateTime = DateTime.Now;
    private BattleController battleController;
    private void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        anim = this.GetComponent<Animator>();
        playerAttack = this.GetComponent<PlayerAttack>();
        if (GameController.Instance.battleType == BattleType.Team && isCanControl)
        {
            battleController = GameController.Instance.GetComponent<BattleController>();
            InvokeRepeating("SyncPositionAndRotation", 0, 1f / 30); // һ��ͬ����ʮ��
            InvokeRepeating("SyncMoveAnimation", 0, 1f / 30);
        }
    }

    private void Update()
    {
        if (isCanControl == false) return;
        if (playerAttack != null && playerAttack.hp <= 0) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 nowVel = _rigidbody.velocity;
        if(Mathf.Abs(h) > 0.05f || Mathf.Abs(v) > 0.05f)
        {
            anim.SetBool("Move", true);
            if (anim.GetCurrentAnimatorStateInfo(1).IsName("EmptyState")) //����ʱ���ƶ�
            {
                _rigidbody.velocity = new Vector3(velocity * h, nowVel.y, velocity * v);
                transform.LookAt(new Vector3(h, 0, v) + transform.position);
            }
            else
            {
                _rigidbody.velocity = new Vector3(0, nowVel.y, 0);
            }
        }
        else
        {
            anim.SetBool("Move", false);
            _rigidbody.velocity = new Vector3(0, nowVel.y, 0);
        }
    }

    //ͬ����ǰ��ɫ��λ�ú���ת ���������
    void SyncPositionAndRotation()
    {
        Vector3 position = transform.position;
        Vector3 eulerAngles = transform.eulerAngles;
        if (position.x != lastPosition.x || position.y != lastPosition.y || position.z != lastPosition.z ||
            eulerAngles.x != lastEulerAngles.x || eulerAngles.y != lastEulerAngles.y ||
            eulerAngles.z != lastEulerAngles.z)
        {
            //����ͬ��
            battleController.SyncPositionAndRotation(position, eulerAngles);
            lastPosition = position;
            lastEulerAngles = eulerAngles;
        }
    }

    public void SetPositionAndRotation(Vector3 position, Vector3 eulerAngles)
    {
        transform.position = position;
        transform.eulerAngles = eulerAngles;
    }

    void SyncMoveAnimation()//ͬ���ƶ��Ķ���
    {
        if (isMove != anim.GetBool("Move"))//��ǰ����״̬�����˸ı� ��Ҫͬ��
        {
            //����ͬ��������
            PlayerMoveAnimationModel model = new PlayerMoveAnimationModel() { IsMove = anim.GetBool("Move") };
            model.SetTime(DateTime.Now);
            battleController.SyncMoveAnimation(model);
            isMove = anim.GetBool("Move");
        }
    }

    public void SetAnim(PlayerMoveAnimationModel model)
    {
        DateTime dt = model.GetTime();
        if (dt > lastUpdateTime)  //��Ҫ��ȷ��������бȽ�
        {
            anim.SetBool("Move", model.IsMove);
            lastUpdateTime = dt;
        }
    }
}
