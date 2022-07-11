using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerVillageAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody _rigidbody;
    private NavMeshAgent agent;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        _rigidbody = this.GetComponent<Rigidbody>();
        agent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_rigidbody.velocity.magnitude > 0.5f)
        {
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
        if (agent.enabled)
        {
            anim.SetBool("Move", true);
        }
    }
}
