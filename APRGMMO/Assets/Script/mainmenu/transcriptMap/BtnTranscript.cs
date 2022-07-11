using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnTranscript : MonoBehaviour
{
    public int id;
    public int needLevel;
    public int needEnergy = 3;
    public string sceneName;
    public string des = "这里是魔方世界，奥杜因在前面等着你，你敢进入吗？";


    public void OnClick()
    {
        transform.parent.SendMessage("OnBtnTranscriptClick", this);
    }
}
