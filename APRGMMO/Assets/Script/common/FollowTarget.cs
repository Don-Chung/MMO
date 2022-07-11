using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Vector3 offset;
    private Transform playerBip;
    public float smoothing = 4;

    private void Start()
    {
        if(TranscriptManage._instance != null)
        {
            playerBip = TranscriptManage._instance.player.transform.Find("Bip01");
        }
        else
        {
            playerBip = GameObject.FindGameObjectWithTag("Player").transform.Find("Bip01");
        }
 
    }

    private void FixedUpdate()
    {
        //transform.position = playerBip.position + offset;
        Vector3 targetPos = playerBip.position + offset;
        transform.position =  Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }
}
