using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarManager : MonoBehaviour
{
    public static HpBarManager _instance;
    public GameObject hpBarPrefab;
    public GameObject hudTextPrefab;

    private void Awake()
    {
        _instance = this;
    }

    public GameObject GetHpBar(GameObject target)
    {
        GameObject go = NGUITools.AddChild(this.gameObject, hpBarPrefab);
        go.GetComponent<UIFollowTarget>().target = target.transform;
        return go;
    }

    public GameObject GetHudText(GameObject target)
    {
        GameObject go = NGUITools.AddChild(this.gameObject, hudTextPrefab);
        go.GetComponent<UIFollowTarget>().target = target.transform;
        return go;
    }
}
