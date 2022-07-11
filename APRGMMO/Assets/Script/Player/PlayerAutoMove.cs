using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAutoMove : MonoBehaviour
{
    private NavMeshAgent agent;
    public float minDistance = 3f;
    //public Rigidbody rig;

    //public Transform targetPos;

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        //rig = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (agent.enabled)
        {
            if(agent.remainingDistance < minDistance && agent.remainingDistance != 0)
            {
                agent.Stop();
                agent.enabled = false;
                TaskManage._instance.OnArriveDestination();
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (Mathf.Abs(h) > 0.05f || Mathf.Abs(v) > 0.05f)
            {
                //当寻路时按下控制剑，则停止寻路
                StopAuto();
            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    SetDestination(targetPos.position);
        //}
    }

    //private void FixedUpdate()
    //{
    //    Vector3 tmpPos = transform.position;
    //    rig.MovePosition(rig.position);
    //}

    public void SetDestination(Vector3 targetPos)
    {
        agent.enabled = true;
        agent.SetDestination(targetPos);
    }

    public void StopAuto()
    {
        if (agent.enabled)
        {
            agent.Stop();
            agent.enabled = false;
        }
    }
}
