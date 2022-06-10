using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int GetRequireExpByLevel(int level)
    {   //µÈ²îÊýÁÐ
        return (int)((level - 1) * (100f + (100f + 10f * (level - 2f))) / 2f);
    }
}
