using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnTranscript : MonoBehaviour
{
    public int id;
    public int needLevel;
    public int needEnergy = 3;
    public string sceneName;
    public string des = "������ħ�����磬�¶�����ǰ������㣬��ҽ�����";


    public void OnClick()
    {
        transform.parent.SendMessage("OnBtnTranscriptClick", this);
    }
}
