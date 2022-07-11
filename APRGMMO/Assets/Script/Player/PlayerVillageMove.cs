using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerVillageMove : MonoBehaviour
{
    public float velocity = 15;
    private Rigidbody _rigidbody;
    private NavMeshAgent agent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 vel = _rigidbody.velocity;
        if(Mathf.Abs(h) > 0.05f || Mathf.Abs(v) > 0.05f)
        {
                _rigidbody.velocity = new Vector3(-h * velocity, vel.y, -v * velocity);
                transform.rotation = Quaternion.LookRotation(new Vector3(-h, 0, -v));
        }
        //else
        //{
        //    if(agent.enabled == false)
        //    {
        //        _rigidbody.velocity = Vector3.zero;
        //    }
        //}
        if (agent.enabled)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity);
        }
    }
}
